using System.Collections.Generic;
using UnityEngine;

public class InteractionController {
    enum SelectState {
        None,
        Command,
        Zone,
        Card,
    }
    class InteractionContext {
        public Player player;
        public Player opponent;
        public ZoneType curZone = ZoneType.Hand;
        public CardBase selectedCard = null;
        public List<CommandType> commands = null;
        public CommandType selectedCommand = CommandType.None;
    }

    const int DEFALUT_ID = -1;
    GameState _state;
    SelectState _curState;
    InteractionContext _context;

    Command _defaultCommand = new(CommandType.None);
    Command _command = new(CommandType.None);

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


    public Command GetCommand(InputData inputData) {
        if (_curState == SelectState.None) {
            SelcetFree(inputData, _context);
        }
        else if (_curState == SelectState.Command) {
            return SelectCommand(inputData, _context);
        }
        else if (_curState == SelectState.Zone) {
            return SelectZone(inputData, _context);
        }

        return _defaultCommand;
    }

    int _selectCardId = DEFALUT_ID;
    Command SelcetFree(InputData inputData, InteractionContext context) {
        if (inputData[InputType.Left] || inputData[InputType.Right]) {
            var player = context.player;
            var list = player.GetZoneCards(context.curZone);
            _selectCardId += inputData[InputType.Left] ? -1 : 1;
            _selectCardId = Mathf.Clamp(_selectCardId, 0, list.Count - 1);
            Debug.Log($"当前玩家: {player.ID}, 当前选择区域: {context.curZone}, 当前选择id: {_selectCardId}");
            Debug.Log(list[_selectCardId].ShowInfo());
        }

        if (inputData[InputType.Confirm]) {
            var list = context.player.GetZoneCards(context.curZone);
            if (_selectCardId == DEFALUT_ID || list[_selectCardId] == null) {
                Debug.Log("当前无可操作卡");
                return _defaultCommand;
            }
            if (list[_selectCardId] is Card_Monster card) {
                List<CommandType> commands = GetMonsterUsableCommand(card);
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
        }
        return _defaultCommand;
    }

    int _selectCommandId = DEFALUT_ID;
    Command SelectCommand(InputData inputData, InteractionContext context) {
        if (inputData[InputType.Cancel]) {
            ResetState(ref context);
            return _defaultCommand;
        }

        if (inputData[InputType.Left] || inputData[InputType.Right]) {
            _selectCommandId += inputData[InputType.Left] ? -1 : 1;
            _selectCommandId = Mathf.Clamp(_selectCommandId, 0, context.commands.Count - 1);
            Debug.Log($"当前选择id: {_selectCommandId}");
        }

        if (inputData[InputType.Confirm] && _selectCommandId != DEFALUT_ID) {
            if (context.commands[_selectCommandId] == CommandType.NormalSummon) {
                _curState = SelectState.Zone;
                _context.selectedCommand = CommandType.NormalSummon;
                Debug.Log("选择召唤区域");
            }
        }

        return _defaultCommand;

    }

    Command SelectZone(InputData inputData, InteractionContext context) {
        if (inputData[InputType.Cancel]) {
            ResetState(ref context);
            return _defaultCommand;
        }
        return _defaultCommand;
    }

    void ResetState(ref InteractionContext context) {
        _selectCardId = DEFALUT_ID;
        _selectCommandId = DEFALUT_ID;
        context.curZone = ZoneType.Hand;
        context.selectedCard = null;
        context.commands = null;
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
