using System;
using System.Collections.Generic;

public class GameRule {
    public bool CheckAction(ActionType action, CardBase card, ActionContext context) {
        return action switch {
            ActionType.NormalSummon => CheckNormalSummon(card, context),
            ActionType.MonsterSet => CheckMonsterSet(card, context),
            ActionType.SpellTrapSet => CheckSpellTrapSet(card, context),
            ActionType.ChangePosition => CheckChangePosition(card, context),
            ActionType.MonsetFilp => CheckMonsetFilp(card, context),
            ActionType.Attack => CheckMonsterAttack(card, context),
            ActionType.Activate => CheckActivate(card, context),
            _ => true,
        };
    }

    bool CheckNormalSummon(CardBase card, ActionContext context) {
        Phase phase = context.state.CurPhase;
        Player player = context.player;
        if (card.ZoneType == ZoneType.Hand &&
            phase == Phase.Main1 &&
            player.NormalSummonCount < GameDefines.SUMMON_NORMAL_COUNT) {
            return true;
        }

        return false;
    }

    bool CheckMonsterSet(CardBase card, ActionContext context) {
        Phase phase = context.state.CurPhase;
        Player player = context.player;
        if (card.ZoneType == ZoneType.Hand &&
            phase == Phase.Main1 &&
            player.NormalSummonCount < GameDefines.SUMMON_NORMAL_COUNT) {
            return true;
        }

        return false;
    }

    bool CheckSpellTrapSet(CardBase card, ActionContext context) {
        Phase phase = context.state.CurPhase;
        Player player = context.player;
        if (card.ZoneType == ZoneType.Hand &&
            phase == Phase.Main1 &&
            player.NormalSummonCount < GameDefines.SUMMON_NORMAL_COUNT) {
            return true;
        }

        return false;
    }

    bool CheckChangePosition(CardBase card, ActionContext context) {
        Phase phase = context.state.CurPhase;
        Card_Monster monster = card as Card_Monster;
        if (phase == Phase.Main1 &&
            card.ZoneType == ZoneType.Monster &&
            card.Face == CardFace.FaceUp &&
            monster.ChangePositionCount < GameDefines.TURN_CHANGE_POSITION_COUNT) {
            return true;
        }
        return false;
    }

    bool CheckMonsetFilp(CardBase card, ActionContext context) {
        Phase phase = context.state.CurPhase;
        Card_Monster monster = card as Card_Monster;
        if (phase == Phase.Main1 &&
            card.ZoneType == ZoneType.Monster &&
            card.Face == CardFace.FaceDown &&
            monster.ChangePositionCount < GameDefines.TURN_CHANGE_POSITION_COUNT) {
            return true;
        }
        return false;
    }

    bool CheckMonsterAttack(CardBase card, ActionContext context) {
        Phase phase = context.state.CurPhase;
        Card_Monster monster = card as Card_Monster;
        if (phase == Phase.Battle &&
            card.ZoneType == ZoneType.Monster &&
            monster.AttackCount < GameDefines.TURN_ATTACK_COUNT) {
            return true;
        }
        return false;
    }

    bool CheckActivate(CardBase card, ActionContext context) {
        return card.HasEffect;
    }
}