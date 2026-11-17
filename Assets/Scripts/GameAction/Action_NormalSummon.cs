using System;
using System.Collections.Generic;
using UnityEngine;

public class Action_NormalSummon : IPlayerAction {
    public ActionType Type { get; } = ActionType.NormalSummon;

    public bool CanUse(CardBase card, Player player, GameState state) {
        Phase phase = state.CurPhase;
        if (card.ZoneType == ZoneType.Hand &&
            phase == Phase.Main1 &&
            player.NormalSummonCount < GameDefines.SUMMON_NORMAL_COUNT) {
            return true;
        }

        return false;
    }

    public ActionStep GetNextStep(ActionContext ctx) {
        return ActionStep.Zone;
    }

    public IReadOnlyList<CardBase> GetTargetOptions(ActionContext ctx) {
        return new List<CardBase>();
    }

    public IReadOnlyList<int> GetZoneOptions(ActionContext ctx) {
        return ctx.player.GetAvailableZone(ZoneType.Monster);
    }

    public void Execute(ActionContext ctx, DuelAction duelAction) {
        var card = ctx.selectedCard as Card_Monster;
        var player = ctx.player;
        var toZoneId = ctx.toZoneId;
        player.OnNormalSummon();
        card.OnNormalSummon();
        player.MoveCard(card, ZoneType.Monster, toZoneId);
        Debug.Log(string.Format(TextData.Instance.GetText(Text_ID.NormalSummon), player.ID, card.Name, toZoneId));
    }

}
