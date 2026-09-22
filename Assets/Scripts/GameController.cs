using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController {
    GameState _gameState;
    EventSystem _eventSystem;
    public GameController(GameState state, EventSystem eventSystem) {
        _gameState = state;
        _eventSystem = eventSystem;
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
        var attackMonster = command.AttackMonster;
        var targetMonster = command.TargetMonster;
        AttackContext context = new() {
            attacker = attacker,
            opponent = opponent,
            attackMonster = attackMonster,
            targetMonster = targetMonster,
            isDirectAttack = command.IsDirectAttack,
        };
        if (command.IsDirectAttack) {
            Debug.Log(TextData.Instance.GetText(Text_ID.DirectAttack));
            BeforeAttack(context);
            context.damage = attackMonster.Atk;
            context.getDamagePlayer = opponent;
            TakeDamageByAttack(context);
            AfterAttack(context);
            return;
        }

        Debug.Log(TextData.Instance.GetFormatText(Text_ID.Attack_1, attacker.ID, attackMonster.Name, opponent.ID, targetMonster.Name));
        BeforeAttack(context);
        if (targetMonster.Position == MonterPosition.Attack) {
            int atk1 = attackMonster.Atk, atk2 = targetMonster.Atk;
            Debug.Log(TextData.Instance.GetFormatText(Text_ID.Attack_2, attackMonster.Name, atk1, targetMonster.Name, atk2));
            if (atk1 > atk2) {
                DestroyCard(opponent, targetMonster);
                context.damage = atk1 - atk2;
                context.getDamagePlayer = opponent;
                TakeDamageByAttack(context);
            }
            else if (atk2 > atk1) {
                DestroyCard(attacker, attackMonster);
                context.damage = atk2 - atk1;
                context.getDamagePlayer = attacker;
                TakeDamageByAttack(context);
            }
            else {
                DestroyCard(attacker, attackMonster);
                DestroyCard(opponent, targetMonster);
            }
        }
        else {
            int atk = attackMonster.Atk, def = targetMonster.Def;
            Debug.Log(TextData.Instance.GetFormatText(Text_ID.Attack_3, attackMonster.Name, atk, targetMonster.Name, def));
            if (atk > def) {
                DestroyCard(opponent, targetMonster);
            }
            else if (def > atk) {
                attacker.TakeDamage(def - atk);
                context.damage = def - atk;
                context.getDamagePlayer = attacker;
                TakeDamageByAttack(context);
            }
        }
        AfterAttack(context);
    }

    void BeforeAttack(AttackContext context) {
        context.attackMonster.BeforeAttack();
    }

    void TakeDamageByAttack(AttackContext context) {
        context.getDamagePlayer.TakeDamage(context.damage);
        bool attackSuccess = context.getDamagePlayer == context.opponent;
        var info = new DamageEvent(){
            type = DamageType.Battle,
            source = attackSuccess ? context.attacker : context.opponent,
            target = attackSuccess ? context.opponent : context.attacker,
            sourceCard = attackSuccess ? context.attackMonster : context.targetMonster,
            damage = context.damage
        };
        _eventSystem.Trigger(info);
    }

    void AfterAttack(AttackContext context) {
        context.attackMonster.AfterAttack();
    }

    void DestroyCard(Player player, CardBase card) {
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