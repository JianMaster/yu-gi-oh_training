using System;
using System.Collections.Generic;
using UnityEngine;

public class InteractionController {
    enum SelectState {
        None,
        Action,
        Zone,
        Target,
    }
    class InteractionContext {
        public Player player;
        public Player opponent;
        public ZoneType curZone = ZoneType.Hand;
        public CardBase selectedCard = null;
        public List<ActionType> actions = null;
        public ActionType selectedAction = ActionType.None;
        public List<int> monsterZoneId = null;
        public List<CardBase> targets = null;
    }

    const int DEFALUT_ID = -1;
    GameState _state;
    ActionQuery _actionQuery;
    SelectState _curState;
    InteractionContext _context;

    Action _defaultAction = new();

    public InteractionController(GameState state, ActionQuery query) {
        _state = state;
        _actionQuery = query;
        _curState = SelectState.None;
        ChangePlayer(_state.TurnOwner);
    }

    public void ChangePlayer(Player player) {
        _context = new() {
            player = player,
            opponent = _state.GetOpponent(player)
        };
    }

    bool TryGetSelect(InputData inputData, ref int selectId, int count) {
        if (inputData[InputType.Left] || inputData[InputType.Right]) {
            selectId += inputData[InputType.Left] ? -1 : 1;
            selectId = Mathf.Clamp(selectId, 0, count - 1);
            return true;
        }
        return false;
    }

    bool TryGetSelectZone(InputData inputData, ref ZoneType zone) {
        if (inputData[InputType.Up] || inputData[InputType.Down]) {
            zone += inputData[InputType.Up] ? 1 : -1;
            zone = (ZoneType)Mathf.Clamp((int)zone, 1, Enum.GetValues(typeof(ZoneType)).Length - 1);
            return true;
        }
        return false;
    }

    public Action GetAction(InputData inputData) {
        if (inputData[InputType.Cancel]) {
            ResetState(ref _context);
            return _defaultAction;
        }

        if (_curState == SelectState.None) {
            Selcet(inputData, _context);
            if (inputData[InputType.NextPhase]) {
                return new Action() {
                    Type = ActionType.NextPhase,
                    Excuter = _context.player
                };
            }
        }
        else if (_curState == SelectState.Action) {
            return SelectAction(inputData, _context);
        }
        else if (_curState == SelectState.Zone) {
            return SelectZone(inputData, _context);
        }
        else if (_curState == SelectState.Target) {
            return SelectTarget(inputData, _context);
        }

        return _defaultAction;
    }

    int _selectCardId = DEFALUT_ID;
    Action Selcet(InputData inputData, InteractionContext context) {
        if (TryGetSelectZone(inputData, ref context.curZone)) {
            Debug.Log($"当前选择区域: {context.curZone}");
        }
        if (TryGetSelect(inputData, ref _selectCardId, context.player.GetZoneCards(context.curZone).Count)) {
            var player = context.player;
            var list = player.GetZoneCards(context.curZone);
            Debug.Log($"当前玩家: {player.ID}, 当前选择区域: {context.curZone}, 当前选择id: {_selectCardId}");
            Debug.Log(list[_selectCardId] == null ? "当前区域没有卡牌" : list[_selectCardId].ShowInfo());
        }

        if (inputData[InputType.Confirm]) {
            var list = context.player.GetZoneCards(context.curZone);
            if (_selectCardId == DEFALUT_ID || list[_selectCardId] == null) {
                Debug.Log("当前无可操作卡");
                return _defaultAction;
            }
            var card = list[_selectCardId]; // 当前选择的卡牌
            ActionContext actionContext = new() {
                state = _state,
                player = context.player,
            };
            var actions = _actionQuery.GetAvailableAction(card, actionContext);
            if (actions.Count != 0) {
                context.selectedCard = card;
                context.actions = actions;
                _curState = SelectState.Action;
                Debug.Log($"进入指令选择，当前可用指令: {string.Join(" ", actions)}");
            }
            else {
                Debug.Log("当前无可操作选项");
            }

        }
        return _defaultAction;
    }

    int _selectActionId = DEFALUT_ID;
    Action SelectAction(InputData inputData, InteractionContext context) {
        if (TryGetSelect(inputData, ref _selectActionId, context.actions.Count)) {
            // 选择指令
            Debug.Log(TextData.Instance.GetFormatText(Text_ID.SelectCommand_1, context.actions[_selectActionId]));
        }

        if (inputData[InputType.Confirm] && _selectActionId != DEFALUT_ID) {
            ActionType commandType = context.actions[_selectActionId];
            context.selectedAction = commandType;
            if (commandType == ActionType.NormalSummon) {
                _curState = SelectState.Zone;
                context.monsterZoneId = context.player.GetAvailableMonsterZone();
                // 通常召唤
                Debug.Log(TextData.Instance.GetText(Text_ID.SelectCommand_2));
            }
            else if (commandType == ActionType.Attack) {
                _curState = SelectState.Target;
                context.targets = context.opponent.GetAttackTarget();
                foreach (var card in context.targets) {
                    Debug.Log(TextData.Instance.GetFormatText(Text_ID.GetAttackTarget, card.ZoneId, card.Name));
                }
            }
        }

        return _defaultAction;

    }

    int _selectTargetId = DEFALUT_ID;
    Action SelectTarget(InputData inputData, InteractionContext context) {
        if (TryGetSelect(inputData, ref _selectTargetId, context.targets.Count)) {
            Debug.Log(TextData.Instance.GetFormatText(Text_ID.SelectCommand_1, context.targets[_selectTargetId].Name));
        }
        if (inputData[InputType.Confirm] && _selectTargetId != DEFALUT_ID) {
            Action action = CommandFactory.CreateAttack(
                context.player,
                context.opponent,
                context.selectedCard as Card_Monster,
                context.targets[_selectTargetId] as Card_Monster,
                context.targets[_selectTargetId].ZoneId == GameDefines.PLAYER_ZONE
            );
            Debug.Log($"执行指令: {action.Type}");
            ResetState(ref context);
            return action;
        }
        return _defaultAction;
    }

    int _selectZoneId = DEFALUT_ID;
    Action SelectZone(InputData inputData, InteractionContext context) {
        if (TryGetSelect(inputData, ref _selectZoneId, context.monsterZoneId.Count)) {
            Debug.Log(TextData.Instance.GetFormatText(Text_ID.SelectCommand_1, context.monsterZoneId[_selectZoneId]));
        }

        if (inputData[InputType.Confirm] && _selectZoneId != DEFALUT_ID) {
            Action action = CommandFactory.CreateNormalSummon(
                context.player,
                context.selectedCard,
                context.monsterZoneId[_selectZoneId]
            );
            Debug.Log($"执行指令: {action.Type}");
            ResetState(ref context);
            return action;
        }
        return _defaultAction;
    }

    void ResetState(ref InteractionContext context) {
        _curState = SelectState.None;
        _selectCardId = DEFALUT_ID;
        _selectActionId = DEFALUT_ID;
        _selectZoneId = DEFALUT_ID;
        context.curZone = ZoneType.Hand;
        context.selectedCard = null;
        context.actions = null;
        context.selectedAction = ActionType.None;
        context.monsterZoneId = null;
        context.targets = null;
        Debug.Log("重置状态");
    }
}
