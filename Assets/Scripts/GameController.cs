using System;
using System.Collections.Generic;
using UnityEngine;

public class GameController {
    GameState _gameState;
    bool _selectMode;
    int _selectHand = -1;
    bool IsSelectHand => _selectHand != -1;
    int _selectMonster = -1;
    bool IsSelectMonster => _selectMonster != -1;

    public GameController(bool first, PlayerData data1, PlayerData data2) {
        _gameState = new(first, data1, data2);
        _selectMode = false;
    }

    public void GameStart() {
        Debug.Log("游戏开始！");
        _gameState.TurnOwner.Draw(GameDefines.START_CARD_COUNT);
        _gameState.Opponent.Draw(GameDefines.START_CARD_COUNT);
    }

    List<int> _canSelect = new();
    public void ExcuteCommand(InputData command) {
        Player player = _gameState.TurnOwner;
        Player opponent = _gameState.Opponent;

        if (command.cancel) {
            ResetState();
            return;
        }

        if (_gameState.CurPhase == Phase.Draw) {
            player.TurnStart();
            if (_gameState.Turn != 1) {
                player.Draw(GameDefines.DRAW_CARD_COUNT);
            }
            NextPhase(); // 抽牌结束自动下一个阶段
        }

        if (_gameState.CurPhase == Phase.Main1) {
            if (_selectMode) {
                if (command.IsSelect && _canSelect.Contains(command.select)) {
                    player.NormalSummon(_selectHand, command.select);
                    ResetState();
                }
                return;
            }
            if (command.IsSelect) {
                if (player.CheckHand(command.select)) {
                    return;
                }
                _selectHand = command.select;
                Debug.Log($"player{player.ID}, 点击手牌：{_selectHand}");
            }

            if (player.CanNormalSummon && command.normalSummon && IsSelectHand) {
                Debug.Log("通常召唤怪兽，选择区域");
                _canSelect = player.GetAvailableMonsterZone();
                _selectMode = true;
            }
        }

        // 战斗阶段
        if (_gameState.CurPhase == Phase.Battle) {
            if (_selectMode) {
                if (command.IsSelect && _canSelect.Contains(command.select)) {
                    player.Attack(_selectHand, opponent, command.select);
                    ResetState();
                }
                return;
            }
            if (command.Attack && command.IsSelect) {
                if (!player.CheckMonsterCanAttack(command.select)) {
                    Debug.Log("没有可攻击的怪兽");
                    return;
                }
                Debug.Log("准备攻击");
                _canSelect = opponent.GetAttackTarget();
                _selectMode = true;
                _selectMonster = command.select;
            }
        }

        if (_gameState.CurPhase == Phase.End) {
            player.CheckHandLimit();
            NextPhase();
        }

        if (command.nextPhase) {
            NextPhase();
            return;
        }
    }

    void NextPhase() {
        ResetState();
        _gameState.NextPhase();
    }

    void ResetState() {
        _selectMode = false;
        _selectHand = -1;
        _selectMonster = -1;
        _canSelect.Clear();
    }


}