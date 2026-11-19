using System;
using System.Collections.Generic;
using UnityEngine;

public class Action_Attack : IPlayerAction {
    public ActionType Type { get; } = ActionType.Attack;

    public bool CanUse(CardBase card, Player player, GameState state) {
        Phase phase = state.CurPhase;
        Card_Monster monster = card as Card_Monster;
        if (phase == Phase.Battle &&
            monster.Zone.type == ZoneType.Monster &&
            monster.Position == MonterPosition.Attack &&
            monster.AttackCount < GameDefines.TURN_ATTACK_COUNT) {
            return true;
        }
        return false;
    }

    public void Execute(ActionContext ctx, DuelAction duelAction) {
        var attacker = ctx.player;
        var defender = ctx.opponent;
        var attackMonster = ctx.selectedCard as Card_Monster;
        var targetMonster = ctx.targetCard as Card_Monster;
        ctx.damageType = DamageType.Battle;

        if (ctx.isDirectAttack) {
            Debug.Log(TextData.Instance.GetText(Text_ID.DirectAttack));
            BeforeAttack(ctx);
            ctx.damage = attackMonster.Atk;
            ctx.damageSource = attacker;
            ctx.damageTarget = defender;
            duelAction.Damage(defender, attackMonster.Atk, ctx);
            AfterAttack(ctx);
            return;
        }

        Debug.Log(TextData.Instance.GetFormatText(Text_ID.Attack_1, attacker.ID, attackMonster.Name, defender.ID, targetMonster.Name));
        BeforeAttack(ctx);
        if (targetMonster.Position == MonterPosition.Attack) {
            int atk1 = attackMonster.Atk, atk2 = targetMonster.Atk;
            int damage = Math.Abs(atk1 - atk2);
            ctx.damage = damage;
            Debug.Log(TextData.Instance.GetFormatText(Text_ID.Attack_2, attackMonster.Name, atk1, targetMonster.Name, atk2));
            if (atk1 > atk2) {
                ctx.damageSource = attacker;
                ctx.damageTarget = defender;
                duelAction.Damage(defender, damage, ctx);
                duelAction.Destroy(targetMonster);
            }
            else if (atk2 > atk1) {
                ctx.damageSource = defender;
                ctx.damageTarget = attacker;
                duelAction.Damage(attacker, damage, ctx);
                duelAction.Destroy(attackMonster);
            }
            else {
                duelAction.Destroy(attackMonster);
                duelAction.Destroy(targetMonster);
            }
        }
        else {
            int atk = attackMonster.Atk, def = targetMonster.Def;
            Debug.Log(TextData.Instance.GetFormatText(Text_ID.Attack_3, attackMonster.Name, atk, targetMonster.Name, def));
            if (atk > def) {
                duelAction.Destroy(targetMonster);
            }
            else if (def > atk) {
                int damage = def - atk;
                ctx.damage = damage;
                ctx.damageSource = defender;
                ctx.damageTarget = attacker;
                duelAction.Damage(attacker, damage, ctx);
            }
        }
        AfterAttack(ctx);
    }

    void BeforeAttack(ActionContext ctx) {
        (ctx.selectedCard as Card_Monster).BeforeAttack(true);
        (ctx.targetCard as Card_Monster).BeforeAttack(false);
    }
    void AfterAttack(ActionContext ctx) {
        (ctx.selectedCard as Card_Monster).AfterAttack(true);
        (ctx.targetCard as Card_Monster).AfterAttack(false);
    }

    public IReadOnlyList<CardBase> GetTargetOptions(ActionContext ctx) {
        return ctx.opponent.GetAttackTarget();
    }

    public IReadOnlyList<Zone> GetZoneOptions(ActionContext ctx) {
        return new List<Zone>();
    }

    public ActionStep GetNextStep(ActionContext ctx) {
        return ActionStep.Target;
    }
}
