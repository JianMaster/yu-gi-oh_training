using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController {
    GameState _gameState;
    DuelAction _duelAction;
    public GameController(GameState state) {
        _gameState = state;
    }

    public void GameStart() {
        Debug.Log("游戏开始！");
        _duelAction.Draw(_gameState.TurnOwner, GameDefines.START_CARD_COUNT);
        _duelAction.Draw(_gameState.Opponent, GameDefines.START_CARD_COUNT);
    }

    public void ExcuteCommand(IPlayerAction action, ActionContext ctx) {
        if (ctx.nextPhase) {
            NextPhase();
            return;
        }
        if (action == null) {
            return;
        }

        action.Execute(ctx, _duelAction);
    }


    void NextPhase() {
        _gameState.NextPhase();
        if (_gameState.CurPhase == Phase.Draw && _gameState.Turn == 1) {
            _duelAction.Draw(_gameState.TurnOwner, GameDefines.DRAW_CARD_COUNT);
        }
    }
}