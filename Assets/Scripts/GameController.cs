using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
        if (command.Type == CommandType.NextPhase) {
            NextPhase();
            if (_gameState.CurPhase == Phase.Draw) {
                var player = _gameState.TurnOwner;
                player.TurnStart();
                if (_gameState.Turn != 1) {
                    Draw(player, GameDefines.DRAW_CARD_COUNT);
                }
                ExcuteCommand(CommandFactory.CreateNextPhase(player)); // 抽牌结束自动下一个阶段
            }
            if (_gameState.CurPhase == Phase.End) {
                ExcuteCommand(CommandFactory.CreateNextPhase(_gameState.TurnOwner));
            }
            return;
        }

        if (command is Command_NormalSummon normalSummon) {
            NormalSummon(normalSummon);
        }

        if (command is Command_Attack attack) {
            Attack(attack);
        }
    }

    void Draw(Player player, int count) {
        player.Draw(count);
    }

    void NormalSummon(Command_NormalSummon command) {
        var player = command.Excuter;
        var card = command.TargetCard;
        var zoneId = command.TargetZoneId;
        player.NormalSummon(card as Card_Monster, zoneId);
    }

    void Attack(Command_Attack command) {
        var attacker = command.Excuter;
        var opponent = command.Opponent;
        var selfMonster = command.AttackCard as Card_Monster;
        var opponentMonster = command.TargetCard as Card_Monster;
        if (command.IsDirectAttack) {
            Debug.Log(TextData.Instance.GetText(Text_ID.DirectAttack));
            selfMonster.BeforeAttack();
            opponent.TakeDamage(selfMonster.Atk);
            selfMonster.AfterAttack();
            return;
        }

        Debug.Log(TextData.Instance.GetFormatText(Text_ID.Attack_1, attacker.ID, selfMonster.Name, opponent.ID, opponentMonster.Name));
        selfMonster.BeforeAttack();
        if (opponentMonster.Position == MonterPosition.Attack) {
            int atk1 = selfMonster.Atk, atk2 = opponentMonster.Atk;
            Debug.Log(TextData.Instance.GetFormatText(Text_ID.Attack_2, selfMonster.Name, atk1, opponentMonster.Name, atk2));
            if (atk1 > atk2) {
                DestroyCard(opponent, opponentMonster);
                opponent.TakeDamage(atk1 - atk2);
            }
            else if (atk2 > atk1) {
                DestroyCard(attacker, selfMonster);
                attacker.TakeDamage(atk2 - atk1);
            }
            else {
                DestroyCard(attacker, selfMonster);
                DestroyCard(opponent, opponentMonster);
            }
        }
        else {
            int atk = selfMonster.Atk, def = opponentMonster.Def;
            Debug.Log(TextData.Instance.GetFormatText(Text_ID.Attack_3, selfMonster.Name, atk, opponentMonster.Name, def));
            if (atk > def) {
                DestroyCard(opponent, opponentMonster);
            }
            else if (def > atk) {
                attacker.TakeDamage(def - atk);
            }
        }
        selfMonster.AfterAttack();
    }

    public void DestroyCard(Player player, CardBase card) {
        if (player.ID != card.Belong) {
            Debug.LogError("卡牌不属于该玩家");
            return;
        }
        player.DestroyCard(card);

        Debug.Log(TextData.Instance.GetFormatText(Text_ID.Destroy, player.ID, card.Name));
    }

    void NextPhase() {
        ResetState();
        _gameState.NextPhase();
    }

    void ResetState() {

    }


}