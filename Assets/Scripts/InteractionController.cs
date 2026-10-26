using System;
using System.Collections.Generic;
using UnityEngine;

public class InteractionController {
    const int DEFAULT_ID = -1;
    GameState _state;
    ActionQuery _actionQuery;
    ActionStep _curStep;
    IGameAction _action;
    ActionContext _context;

    public InteractionController(GameState state, ActionQuery query) {
        _state = state;
        _actionQuery = query;
        _curStep = ActionStep.None;
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

    public (IGameAction, ActionContext) GetAction(InputData inputData) {
        if (inputData[InputType.Cancel]) {
            ResetState();
            return (null, null);
        }

        if (inputData[InputType.NextPhase]) {
            return (null, new ActionContext(_state.CurOperator, _state.CurOpponent) { nextPhase = true });
        }

        if (_curStep == ActionStep.None) {
            Select(inputData);
        }
        else if (_curStep == ActionStep.ActionSelect) {
            SelectAction(inputData, _context);
        }
        else if (_curStep == ActionStep.Zone) {
            SelectZone(inputData, _action, _context);
        }
        else if (_curStep == ActionStep.Target) {
            SelectTarget(inputData, _action, _context);
        }


        if (_curStep == ActionStep.Execute) {
            ResetState();
            return (_action, _context);
        }

        return (null, null);
    }

    int _selectCardId = DEFAULT_ID;
    ZoneType _curZone = ZoneType.Hand;
    void Select(InputData inputData) {
        var player = _state.CurOperator;
        if (TryGetSelectZone(inputData, ref _curZone)) {
            _selectCardId = DEFAULT_ID;
            Debug.Log($"当前选择区域: {_curZone}");
        }
        if (TryGetSelect(inputData, ref _selectCardId, player.GetZoneCards(_curZone).Count)) {
            var list = player.GetZoneCards(_curZone);
            Debug.Log($"当前玩家: {player.ID}, 当前选择区域: {_curZone}, 当前选择id: {_selectCardId}");
            Debug.Log(list[_selectCardId] == null ? "当前区域没有卡牌" : list[_selectCardId].ShowInfo());
        }

        if (inputData[InputType.Confirm]) {
            var list = player.GetZoneCards(_curZone);
            if (_selectCardId == DEFAULT_ID || list[_selectCardId] == null) {
                Debug.Log("当前无可操作卡");
                return;
            }
            var card = list[_selectCardId]; // 当前选择的卡牌
            _actions = _actionQuery.GetAvailableAction(card, player, _state);
            if (_actions.Count != 0) {
                _context = new(player, _state.CurOpponent);
                _context.selectedCard = card;
                _curStep = ActionStep.ActionSelect;
                Debug.Log($"进入指令选择，当前可用指令: {string.Join(" ", _actions)}");
            }
            else {
                Debug.Log("当前无可操作选项");
            }

        }
        return;
    }

    int _selectActionId = DEFAULT_ID;
    List<IGameAction> _actions = new();
    void SelectAction(InputData inputData, ActionContext ctx) {
        if (TryGetSelect(inputData, ref _selectActionId, _actions.Count)) {
            // 选择指令
            Debug.Log(TextData.Instance.GetFormatText(Text_ID.SelectCommand_1, _actions[_selectActionId].Type));
        }

        if (inputData[InputType.Confirm] && _selectActionId != DEFAULT_ID) {
            _action = _actions[_selectActionId];
            _curStep = _action.GetNextStep(ctx);
            if (_curStep == ActionStep.Zone) {
                _zoneIds = _action.GetZoneOptions(ctx);
            }
            else if (_curStep == ActionStep.Target) {
                _targets = _action.GetTargetOptions(ctx);
            }
        }
    }

    int _selectTargetId = DEFAULT_ID;
    IReadOnlyList<CardBase> _targets = new List<CardBase>();
    void SelectTarget(InputData inputData, IGameAction action, ActionContext ctx) {
        if (TryGetSelect(inputData, ref _selectTargetId, _targets.Count)) {
            Debug.Log(TextData.Instance.GetFormatText(Text_ID.SelectCommand_1, _targets[_selectTargetId].Name));
        }
        if (inputData[InputType.Confirm] && _selectTargetId != DEFAULT_ID) {
            ctx.targetCard = _targets[_selectTargetId];
            Debug.Log($"执行指令: {action.Type}");
        }
    }

    int _selectZoneId = DEFAULT_ID;
    IReadOnlyList<int> _zoneIds = new List<int>();
    void SelectZone(InputData inputData, IGameAction action, ActionContext ctx) {
        if (TryGetSelect(inputData, ref _selectZoneId, _zoneIds.Count)) {
            Debug.Log(TextData.Instance.GetFormatText(Text_ID.SelectCommand_1, _zoneIds[_selectZoneId]));
        }

        if (inputData[InputType.Confirm] && _selectZoneId != DEFAULT_ID) {
            ctx.toZoneId = _zoneIds[_selectZoneId];
            Debug.Log($"执行指令: {action.Type}");
        }
    }

    void ResetState() {
        _curStep = ActionStep.None;
        _selectCardId = DEFAULT_ID;
        _selectActionId = DEFAULT_ID;
        _selectTargetId = DEFAULT_ID;
        _selectZoneId = DEFAULT_ID;
        Debug.Log("重置状态");
    }
}
