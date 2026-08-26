using System;
using UnityEngine;

public class GameController {
    GameState _gameState;
    bool _selectMode;

    public GameController(bool first) {
        _gameState = new(first);
        _selectMode = false;
    }

    public void GameStart() {
        Debug.Log("游戏开始！");
        _gameState.TurnOwner.Draw(GameDefines.START_CARD_COUNT);
        _gameState.Opponent.Draw(GameDefines.START_CARD_COUNT);
    }

    public void ExcuteCommand(InputData command) {
        if (_selectMode) {
            if (command.cancel) {
                _selectMode = false;
                return;
            }
            
            
        }

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

        if(command.selectHand != 0) {
            _gameState.SetSelectHand(command.selectHand);
        }

        if (command.normalSummon && _gameState.IsSelectHand) {
            Debug.Log("通常召唤怪兽，选择区域");
            _selectMode = true;
        }

    }


}