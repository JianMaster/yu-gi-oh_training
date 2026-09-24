using System;
using System.Collections.Generic;
using UnityEngine;

public class InteractionController {
    enum SelectState {
        None,
        Command,
        Zone,
        Target,
    }
    class InteractionContext {
        public Player player;
        public Player opponent;
        public ZoneType curZone = ZoneType.Hand;
        public CardBase selectedCard = null;
        public List<CommandType> commands = null;
        public CommandType selectedCommand = CommandType.None;
        public List<int> monsterZoneId = null;
        public List<CardBase> targets = null;
    }

    const int DEFALUT_ID = -1;
    GameState _state;
    SelectState _curState;
    InteractionContext _context;

    Command _defaultCommand = new();

    public InteractionController(GameState state) {
        _state = state;
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

    public Command GetCommand(InputData inputData) {
        if (inputData[InputType.Cancel]) {
            ResetState(ref _context);
            return _defaultCommand;
        }

        if (_curState == SelectState.None) {
            Selcet(inputData, _context);
            if (inputData[InputType.NextPhase]) {
                return new Command() {
                    Type = CommandType.NextPhase,
                    Excuter = _context.player
                };
            }
        }
        else if (_curState == SelectState.Command) {
            return SelectCommand(inputData, _context);
        }
        else if (_curState == SelectState.Zone) {
            return SelectZone(inputData, _context);
        }
        else if (_curState == SelectState.Target) {
            return SelectTarget(inputData, _context);
        }

        return _defaultCommand;
    }

    int _selectCardId = DEFALUT_ID;
    Command Selcet(InputData inputData, InteractionContext context) {
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
                return _defaultCommand;
            }
            var card = list[_selectCardId];
            var commands = GetUsableCommand(context.player, card);
            if (commands.Count != 0) {
                context.selectedCard = list[_selectCardId];
                context.commands = commands;
                _curState = SelectState.Command;
                Debug.Log($"进入指令选择，当前可用指令: {string.Join(" ", commands)}");
            }
            else {
                Debug.Log("当前无可操作选项");
            }

        }
        return _defaultCommand;
    }

    int _selectCommandId = DEFALUT_ID;
    Command SelectCommand(InputData inputData, InteractionContext context) {
        if (TryGetSelect(inputData, ref _selectCommandId, context.commands.Count)) {
            // 选择指令
            Debug.Log(TextData.Instance.GetFormatText(Text_ID.SelectCommand_1, context.commands[_selectCommandId]));
        }

        if (inputData[InputType.Confirm] && _selectCommandId != DEFALUT_ID) {
            CommandType commandType = context.commands[_selectCommandId];
            context.selectedCommand = commandType;
            if (commandType == CommandType.NormalSummon) {
                _curState = SelectState.Zone;
                context.monsterZoneId = context.player.GetAvailableMonsterZone();
                // 通常召唤
                Debug.Log(TextData.Instance.GetText(Text_ID.SelectCommand_2));
            }
            else if (commandType == CommandType.Attack) {
                _curState = SelectState.Target;
                context.targets = context.opponent.GetAttackTarget();
                foreach (var card in context.targets) {
                    Debug.Log(TextData.Instance.GetFormatText(Text_ID.GetAttackTarget, card.ZoneId, card.Name));
                }
            }
        }

        return _defaultCommand;

    }

    int _selectTargetId = DEFALUT_ID;
    Command SelectTarget(InputData inputData, InteractionContext context) {
        if (TryGetSelect(inputData, ref _selectTargetId, context.targets.Count)) {
            Debug.Log(TextData.Instance.GetFormatText(Text_ID.SelectCommand_1, context.targets[_selectTargetId].Name));
        }
        if (inputData[InputType.Confirm] && _selectTargetId != DEFALUT_ID) {
            Command command = CommandFactory.CreateAttack(
                context.player,
                context.opponent,
                context.selectedCard as Card_Monster,
                context.targets[_selectTargetId] as Card_Monster,
                context.targets[_selectTargetId].ZoneId == GameDefines.PLAYER_ZONE
            );
            Debug.Log($"执行指令: {command.Type}");
            ResetState(ref context);
            return command;
        }
        return _defaultCommand;
    }

    int _selectZoneId = DEFALUT_ID;
    Command SelectZone(InputData inputData, InteractionContext context) {
        if (TryGetSelect(inputData, ref _selectZoneId, context.monsterZoneId.Count)) {
            Debug.Log(TextData.Instance.GetFormatText(Text_ID.SelectCommand_1, context.monsterZoneId[_selectZoneId]));
        }

        if (inputData[InputType.Confirm] && _selectZoneId != DEFALUT_ID) {
            Command command = CommandFactory.CreateNormalSummon(
                context.player,
                context.selectedCard,
                context.monsterZoneId[_selectZoneId]
            );
            Debug.Log($"执行指令: {command.Type}");
            ResetState(ref context);
            return command;
        }
        return _defaultCommand;
    }

    void ResetState(ref InteractionContext context) {
        _curState = SelectState.None;
        _selectCardId = DEFALUT_ID;
        _selectCommandId = DEFALUT_ID;
        _selectZoneId = DEFALUT_ID;
        context.curZone = ZoneType.Hand;
        context.selectedCard = null;
        context.commands = null;
        context.selectedCommand = CommandType.None;
        context.monsterZoneId = null;
        context.targets = null;
        Debug.Log("重置状态");
    }

    public List<CommandType> GetUsableCommand(Player player, CardBase card) {
        List<CommandType> list = new();
        if (_state.CurPhase == Phase.Main1) {
            if(card.)
            if (card.ZoneType == ZoneType.Hand) {
                if (player.CanNormalSummon && player.GetAvailableMonsterZone().Count != 0) {
                    list.Add(CommandType.NormalSummon);
                    list.Add(CommandType.MonsterSet);
                }
            }
            else if (card.ZoneType == ZoneType.Monster) {
                if (card.CanChangePosition()) {
                    if (card.Face == CardFace.FaceUp) {
                        list.Add(CommandType.ChangePosition);
                    }
                    else {
                        list.Add(CommandType.MonsetFilp);
                    }
                }
            }
        }
        else if (_state.CurPhase == Phase.Battle) {
            if (card is Card_Monster monster) {
                if (card.ZoneType == ZoneType.Monster) {
                    if (monster.CanAttack()) {
                        list.Add(CommandType.Attack);
                    }
                }
            }
        }

        return list;
    }
}
