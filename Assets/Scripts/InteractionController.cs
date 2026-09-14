using System.Collections.Generic;
using UnityEngine;

public class InteractionController {
    enum SelectState {
        None,
        Command,
        Zone,
        Card,
    }
    const int DEFALUT_ID = -1;
    GameState _state;
    ZoneType _curSelectZone = ZoneType.Hand;
    SelectState _selectMode = SelectState.None;

    Command _defaultCommand = new(CommandType.None);
    Command _command = new(CommandType.None);

    public InteractionController(GameState state) {
        _state = state;
    }

    int _selectCardId = DEFALUT_ID;
    List<CommandType> _canSelectCommands = new();
    public Command GetCommand(InputData inputData) {
        if (_selectMode == SelectState.None) {
            SelcetFree(inputData);
        }
        else if (_selectMode == SelectState.Command) {
            return SelectCommand(inputData, _canSelectCommands, _selectCardId);
        }
        else if (_selectMode == SelectState.Zone) {
            return SelectZone(inputData);
        }

        return _defaultCommand;
    }
    
    Command SelcetFree(InputData inputData) {
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
            if (_selectCardId == DEFALUT_ID || list[_selectCardId] == null) {
                Debug.Log("当前无可操作卡");
                return _defaultCommand;
            }
            if (list[_selectCardId] is Card_Monster card) {
                _canSelectCommands = GetMonsterUsableCommand(card);
                if (_canSelectCommands.Count != 0) {
                    _selectMode = SelectState.Command;
                    Debug.Log($"进入指令选择，当前可用指令: {string.Join(" ", _canSelectCommands)}");
                }
                else {
                    Debug.Log("当前无可操作选项");
                }
            }
        }
        return _defaultCommand;
    }

    int _selectCommandId = DEFALUT_ID;
    Command SelectCommand(InputData inputData, List<CommandType> commands, CardBase card) {
        if (inputData[InputType.Cancel]) {
            ResetState();
            return _defaultCommand;
        }

        if (inputData[InputType.Left] || inputData[InputType.Right]) {
            _selectCommandId += inputData[InputType.Left] ? -1 : 1;
            _selectCommandId = Mathf.Clamp(_selectCommandId, 0, commands.Count - 1);
            Debug.Log($"当前选择id: {_selectCommandId}");
        }

        if (inputData[InputType.Confirm] && _selectCommandId != DEFALUT_ID) {
            if (commands[_selectCommandId] == CommandType.NormalSummon) {
                _selectMode = SelectState.Zone;
                Debug.Log("选择召唤区域");
            }
        }

        return _defaultCommand;

    }

    Command SelectZone(InputData inputData) {
        if (inputData[InputType.Cancel]) {
            ResetState();
            return _defaultCommand;
        }
    }

    void ResetState() {
        _selectCardId = DEFALUT_ID;
        _selectCommandId = DEFALUT_ID;
        _selectMode = SelectState.None;
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
