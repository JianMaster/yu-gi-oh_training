using System;
using System.Collections.Generic;
using UnityEngine;

public class Action_Activate : IPlayerAction {
    public ActionType Type { get; } = ActionType.Activate;

    public bool CanUse(CardBase card, Player player, GameState state) {
        Phase phase = state.CurPhase;
        bool pass = phase == Phase.Main1 && card.HasEffect;
        if (card is Card_Trap trap) {
            pass = pass && !trap.SetAndDown;
        }
        return pass;
    }

    public void Execute(ActionContext ctx, DuelAction duelAction) {
        var player = ctx.player;
        var card = ctx.selectedCard;
        var fromZone = card.Zone;
        var toZone = ctx.toZone;
        var effect = card.Effect;
        if (card.Owner != player) {
            Debug.LogError("卡牌不属于该玩家");
            return;
        }

        EffectContext context = new() {
            activater = player,
            opponent = ctx.opponent,
        };
        if (fromZone != toZone) {
            player.MoveCard(card, ZoneType.SpellTrap, toZone.slot);
        }

        if (fromZone == toZone && card.CardType != CardType.Monster) {
            card.Flip();
        }

        effect.Resolve(context);

        if (card.CardType != CardType.Monster) {
            player.MoveCard(card, ZoneType.GY);
        }
    }

    public IReadOnlyList<CardBase> GetTargetOptions(ActionContext ctx) {
        return new List<CardBase>();
    }

    public IReadOnlyList<Zone> GetZoneOptions(ActionContext ctx) {
        return ctx.player.GetAvailableZone(ZoneType.SpellTrap);
    }

    public ActionStep GetNextStep(ActionContext ctx) {
        if (ctx.selectedCard.Zone.type != ZoneType.Hand) {
            return ActionStep.Zone;
        }
        else {
            return ActionStep.Execute;
        }
    }
}
