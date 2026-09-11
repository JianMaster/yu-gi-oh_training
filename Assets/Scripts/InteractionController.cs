using System.Collections.Generic;
using UnityEngine;

public class InteractionController {
    enum SelectType {
        Effect,
        Card,
    }
    GameState _state;
    ZoneType _curSelectZone = ZoneType.Hand;
    int _selectCardId = -1;
    int _selectCommandId = -1;
    bool _selectCommandMode = false;
    List<CardBase> _selectCards = new();
    List<CommandType> _canSelectCommands = new();

    Command _defaultCommand = new(CommandType.None);

    public InteractionController(GameState state) {
        _state = state;
    }

    public Command GetCommand(InputData inputData) {
        if (_selectCommandMode) {
            return SelectCommandMode(inputData);
        }

        if (inputData[InputType.Left] || inputData[InputType.Right]) {
            var player = _state.TurnOwner;
            var list = player.GetZoneCards(_curSelectZone);
            _selectCardId += inputData[InputType.Left] ? -1 : 1;
            _selectCardId = Mathf.Clamp(_selectCardId, 0, list.Count - 1);
            Debug.Log($"当前玩家: {player.ID}, 当前选择区域: {_curSelectZone}, 当前选择id: {_selectCardId}");
            Debug.Log(list[_selectCardId].ShowInfo());
        }

        if (inputData[InputType.Confirm]) {
            var player = _state.TurnOwner;
            var list = player.GetZoneCards(_curSelectZone);
            if (_selectCardId == -1 || list[_selectCardId] == null) {
                Debug.Log("当前无可操作卡");
                return _defaultCommand;
            }
            if (list[_selectCardId] is Card_Monster card) {
                _canSelectCommands = GetMonsterUsableCommand(card);
                if (_canSelectCommands.Count != 0) {
                    _selectCommandMode = true;
                }
                else {
                    Debug.Log("当前无可操作选项");
                }
            }
        }


        return _defaultCommand;
    }

    Command SelectCommandMode(InputData inputData) {
        if (inputData[InputType.Cancel]) {
            ResetState();
            return _defaultCommand;
        }

        if (inputData[InputType.Left] || inputData[InputType.Right]) {
            var player = _state.TurnOwner;
            _selectCommandId += inputData[InputType.Left] ? -1 : 1;
            _selectCommandId = Mathf.Clamp(_selectCommandId, 0, _canSelectCommands.Count - 1);
            Debug.Log($"当前可用指令: {string.Join(" ", _canSelectCommands)}, 当前选择id: {_selectCommandId}");
        }

        if (inputData[InputType.Confirm]) {
            var player = _state.TurnOwner;
            var list = player.GetZoneCards(_curSelectZone);
            if (_selectCommandId == -1 || list[_selectCardId] == null) {
                Debug.Log("当前无可操作卡");
                return _defaultCommand;
            }
            if (list[_selectCardId] is Card_Monster card) {
                _canSelectCommands = GetMonsterUsableCommand(card);
                if (_canSelectCommands.Count != 0) {
                    _selectCommandMode = true;
                }
                else {
                    Debug.Log("当前无可操作选项");
                }
            }
            return _defaultCommand;
        }

        return _defaultCommand;

    }

    void ResetState() {
        _selectCardId = -1;
        _selectCommandId = -1;
        _selectCommandMode = false;
        _curSelectZone = ZoneType.Hand;
        Debug.Log("取消操作");
    }

    public List<CommandType> GetMonsterUsableCommand(Card_Monster card) {
        List<CommandType> list = new();
        if (card.ZoneType == ZoneType.Hand) {
            list.Add(CommandType.NormalSummon);
            list.Add(CommandType.MonsterSet);
        }
        else if (card.ZoneType == ZoneType.Monster) {
            if (card.Position == MonterPosition.Attack) {
                list.Add(CommandType.Attack);
            }

            if (card.Face == CardFace.FaceUp) {
                list.Add(CommandType.ChangePosition);
            }
            else {
                list.Add(CommandType.MonsetFilp);
            }
        }

        return list;
    }
}
