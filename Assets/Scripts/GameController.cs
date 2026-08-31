using System;
using System.Collections.Generic;
using UnityEngine;

public class GameController {
    GameState _gameState;
    bool _selectMode;

    public GameController(bool first, PlayerData data1, PlayerData data2) {
        _gameState = new(first, data1, data2);
        _selectMode = false;
    }

    public void GameStart() {
        Debug.Log("游戏开始！");
        _gameState.TurnOwner.Draw(GameDefines.START_CARD_COUNT);
        _gameState.Opponent.Draw(GameDefines.START_CARD_COUNT);
    }

    List<int> _monsterZone = new();
    public void ExcuteCommand(InputData command) {
        Player player = _gameState.TurnOwner;
        if (command.cancel) {
            ResetState(player);
            return;
        }

        if (_gameState.CurPhase == Phase.Main1) {
            if (_selectMode) {
                if (command.IsSelect && _monsterZone.Contains(command.select)) {
                    player.NormalSummon(command.select);
                    ResetState(player);
                }
                return;
            }
            if (command.IsSelect) {
                player.SetSelectHand(command.select);
            }

            if (player.CanNormalSummon && command.normalSummon && player.IsSelectHand) {
                Debug.Log("通常召唤怪兽，选择区域");
                _monsterZone = player.GetAvailableMonsterZone();
                _selectMode = true;
            }
        }

        if (command.nextPhase) {
            ResetState(player);
            _gameState.NextPhase();
            if (_gameState.CurPhase == Phase.Draw) {
                _gameState.TurnOwner.TurnStart();
                _gameState.TurnOwner.Draw(1);
            }
            else if (_gameState.CurPhase == Phase.End) {
                player.CheckHandLimit();
            }
        }

    }

    void ResetState(Player player = null) {
        _selectMode = false;
        _monsterZone.Clear();
        player?.SetSelectHand(-1);
    }


}