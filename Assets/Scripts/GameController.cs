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

    public void ExcuteCommand(IGameAction action, ActionContext ctx) {
        if (ctx.nextPhase) {
            NextPhase();
            return;
        }
        if (action == null) {
            return;
        }

        action.Execute(ctx);
    }

    // void Activate(Action_Activate activate) {
    //     var player = activate.Executer;
    //     var card = activate.SelectCard;
    //     var toZoneId = activate.toZoneId;
    //     var fromZone = card.ZoneType;
    //     var toZone = activate.toZone;
    //     var effect = card.Effect;
    //     if (card.Owner != player) {
    //         Debug.LogError("卡牌不属于该玩家");
    //         return;
    //     }

    //     EffectContext context = new() {
    //         activater = player,
    //         opponent = _gameState.GetOpponent(player),
    //     };
    //     if (fromZone != toZone) {
    //         player.MoveCard(card, ZoneType.SpellTrap, toZoneId);
    //     }

    //     if (fromZone == toZone && card.CardType != CardType.Monster) {
    //         card.Flip();
    //     }

    //     effect.Resolve(context);

    //     if (card.CardType != CardType.Monster) {
    //         player.MoveCard(card, ZoneType.GY);
    //     }
    // }

    void Draw(Player player, int count) {
        player.Draw(count);
    }

    // void Attack(Action_Attack action) {
    //     var attacker = action.Executer;
    //     var opponent = action.Opponent;
    //     var attackMonster = action.AttackMonster;
    //     var targetMonster = action.TargetMonster;
    //     AttackContext context = new() {
    //         attacker = attacker,
    //         opponent = opponent,
    //         attackMonster = attackMonster,
    //         targetMonster = targetMonster,
    //         isDirectAttack = action.IsDirectAttack,
    //     };
    //     if (action.IsDirectAttack) {
    //         Debug.Log(TextData.Instance.GetText(Text_ID.DirectAttack));
    //         BeforeAttack(context);
    //         context.damage = attackMonster.Atk;
    //         context.getDamagePlayer = opponent;
    //         TakeDamageByAttack(context);
    //         AfterAttack(context);
    //         return;
    //     }

    //     Debug.Log(TextData.Instance.GetFormatText(Text_ID.Attack_1, attacker.ID, attackMonster.Name, opponent.ID, targetMonster.Name));
    //     BeforeAttack(context);
    //     if (targetMonster.Position == MonterPosition.Attack) {
    //         int atk1 = attackMonster.Atk, atk2 = targetMonster.Atk;
    //         Debug.Log(TextData.Instance.GetFormatText(Text_ID.Attack_2, attackMonster.Name, atk1, targetMonster.Name, atk2));
    //         if (atk1 > atk2) {
    //             DestroyCard(opponent, targetMonster);
    //             context.damage = atk1 - atk2;
    //             context.getDamagePlayer = opponent;
    //             TakeDamageByAttack(context);
    //         }
    //         else if (atk2 > atk1) {
    //             DestroyCard(attacker, attackMonster);
    //             context.damage = atk2 - atk1;
    //             context.getDamagePlayer = attacker;
    //             TakeDamageByAttack(context);
    //         }
    //         else {
    //             DestroyCard(attacker, attackMonster);
    //             DestroyCard(opponent, targetMonster);
    //         }
    //     }
    //     else {
    //         int atk = attackMonster.Atk, def = targetMonster.Def;
    //         Debug.Log(TextData.Instance.GetFormatText(Text_ID.Attack_3, attackMonster.Name, atk, targetMonster.Name, def));
    //         if (atk > def) {
    //             DestroyCard(opponent, targetMonster);
    //         }
    //         else if (def > atk) {
    //             context.damage = def - atk;
    //             context.getDamagePlayer = attacker;
    //             TakeDamageByAttack(context);
    //         }
    //     }
    //     AfterAttack(context);
    // }

    // void AfterAttack(AttackContext context) {
    //     context.attackMonster.AfterAttack();
    // }
    // void BeforeAttack(AttackContext context) {
    //     context.attackMonster.BeforeAttack();
    // }

    // void TakeDamageByAttack(AttackContext context) {
    //     context.getDamagePlayer.TakeDamage(context.damage);
    //     bool attackSuccess = context.getDamagePlayer == context.opponent;
    //     var info = new DamageEvent() {
    //         type = DamageType.Battle,
    //         source = attackSuccess ? context.attacker : context.opponent,
    //         target = attackSuccess ? context.opponent : context.attacker,
    //         sourceCard = attackSuccess ? context.attackMonster : context.targetMonster,
    //         damage = context.damage
    //     };
    //     EventSystem.Trigger(info);
    // }

    void DestroyCard(Player player, CardBase card) {
        if (player != card.Owner) {
            Debug.LogError("卡牌不属于该玩家");
            return;
        }
        player.MoveCard(card, ZoneType.GY);

        Debug.Log(TextData.Instance.GetFormatText(Text_ID.Destroy, player.ID, card.Name));
    }

    void NextPhase() {
        _gameState.NextPhase();
        if (_gameState.CurPhase == Phase.Draw && _gameState.Turn == 1) {
            Draw(_gameState.TurnOwner, GameDefines.DRAW_CARD_COUNT);
        }
    }
}