using System;
using System.Collections.Generic;
using UnityEngine;

public class GameController {
    GameState _gameState;
    public GameController(GameState state) {
        _gameState = state;
    }

    public void GameStart() {
        Debug.Log("游戏开始！");
        Draw(_gameState.TurnOwner, GameDefines.START_CARD_COUNT);
        Draw(_gameState.Opponent, GameDefines.START_CARD_COUNT);
    }

    public void ExcuteCommand(Command command) {
        if (command.Type == CommandType.None) {
            return;
        }

        if (command.Type == CommandType.NormalSummon) {
            NormalSummon(command.Excuter, command.TargetCard, command.TargetZoneId);
        }

        if (command.Type == CommandType.Attack) {

        }

        // 战斗阶段
        // if (_gameState.CurPhase == Phase.Battle) {
        //     if (_selectMode) {
        //         if (command.IsSelect && _canSelect.Contains(command.select)) {
        //             player.Attack(_selectMonster, opponent, command.select);
        //             ResetState();
        //         }
        //         return;
        //     }
        //     if (command.Attack && command.IsSelect) {
        //         if (!player.CheckMonsterCanAttack(command.select)) {
        //             Debug.Log("没有可攻击的怪兽");
        //             return;
        //         }
        //         Debug.Log($"怪兽区域{command.select}准备攻击");
        //         _canSelect = opponent.GetAttackTarget();
        //         _selectMode = true;
        //         _selectMonster = command.select;
        //     }
        // }

        if (command.Type == CommandType.NextPhase) {
            NextPhase();
            if (_gameState.CurPhase == Phase.Draw) {
                var player = _gameState.TurnOwner;
                player.TurnStart();
                if (_gameState.Turn != 1) {
                    Draw(player, GameDefines.DRAW_CARD_COUNT);
                }
                NextPhase(); // 抽牌结束自动下一个阶段
            }
            if (_gameState.CurPhase == Phase.End) {
                NextPhase();
            }
            return;
        }
    }

    void Draw(Player player, int count) {
        player.Draw(count);
    }

    void NormalSummon(Player player, CardBase card, int zoneId) {
        player.NormalSummon(card as Card_Monster, zoneId);
    }

    void Attack() {
        
    }
    void NextPhase() {
        ResetState();
        _gameState.NextPhase();
    }

    void ResetState() {

    }


}