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
        public List<int> monsterZoneId = null;
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
            selectId = Mathf.Clamp(selectId, 0, count);
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

        return _defaultCommand;
    }

    int _selectCardId = DEFALUT_ID;
    Command Selcet(InputData inputData, InteractionContext context) {
        if (TryGetSelect(inputData, ref _selectCardId, context.player.GetZoneCards(context.curZone).Count - 1)) {
            var player = context.player;
            var list = player.GetZoneCards(context.curZone);
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
                List<CommandType> commands = GetMonsterUsableCommand(context.player, card);
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
        if (TryGetSelect(inputData, ref _selectCommandId, context.commands.Count - 1)) {
            Debug.Log($"当前选择id: {_selectCommandId}");
        }

        if (inputData[InputType.Confirm] && _selectCommandId != DEFALUT_ID) {
            if (context.commands[_selectCommandId] == CommandType.NormalSummon) {
                _curState = SelectState.Zone;
                context.selectedCommand = CommandType.NormalSummon;
                context.monsterZoneId = context.player.GetAvailableMonsterZone();
                Debug.Log("选择召唤区域");
            }
        }

        return _defaultCommand;

    }

    int _selectZoneId = DEFALUT_ID;
    Command SelectZone(InputData inputData, InteractionContext context) {
        if (TryGetSelect(inputData, ref _selectZoneId, context.monsterZoneId.Count - 1)) {
            Debug.Log($"当前选择区域: {_selectZoneId}");
        }

        if (inputData[InputType.Confirm] && _selectZoneId != DEFALUT_ID) {
            Command command = new() {
                Type = context.selectedCommand,
                Excuter = context.player,
                TargetCard = context.selectedCard,
                TargetZoneId = _selectZoneId
            };
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
        Debug.Log("重置状态");
    }

    public List<CommandType> GetMonsterUsableCommand(Player player, Card_Monster card) {
        List<CommandType> list = new();
        if (_state.CurPhase == Phase.Main1) {
            if (card.ZoneType == ZoneType.Hand) {
                if (player.CanNormalSummon && player.GetAvailableMonsterZone().Count != 0) {
                    list.Add(CommandType.NormalSummon);
                    list.Add(CommandType.MonsterSet);
                }
            }
        }
        else if (_state.CurPhase == Phase.Battle) {
            if (card.ZoneType == ZoneType.Monster) {
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
        }

        return list;
    }
}
