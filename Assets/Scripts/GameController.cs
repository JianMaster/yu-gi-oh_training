using System;
using UnityEngine;

public class GameController {
    GameState _gameState;

    public GameController(bool first) {
        _gameState = new(first);
    }

    public void GameStart() {
        Debug.Log("游戏开始！");
        _gameState.TurnOwner.Draw(GameDefines.START_CARD_COUNT);
        _gameState.Opponent.Draw(GameDefines.START_CARD_COUNT);
    }

    public void Update(InputData command) {
        if (command.nextPhase) {
            _gameState.NextPhase();
            Player player = _gameState.TurnOwner;
            if (_gameState.CurPhase == Phase.Draw) {
                player.Draw(1);
            }
            else if (_gameState.CurPhase == Phase.End) {
                player.CheckHandLimit();
            }
        }

    }


}