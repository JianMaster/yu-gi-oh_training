using System;
using System.Collections.Generic;
using UnityEngine;

public class Action_Activate : IPlayerAction {
    public ActionType Type { get; } = ActionType.Activate;
    public CardBase SelectCard { get; set; }
    public int toZoneId { get; set; }
    public ZoneType toZone { get; set; }

    public bool CanUse(CardBase card, Player player, GameState state) {
        Phase phase = state.CurPhase;
        bool pass = phase == Phase.Main1 && card.HasEffect;
        if (card is Card_Trap trap) {
            pass = pass && !trap.SetAndDown;
        }
        return pass;
    }

    public void Execute(ActionContext ctx, DuelAction duelAction) {
        throw new System.NotImplementedException();
    }

    public IReadOnlyList<CardBase> GetTargetOptions(ActionContext ctx) {
        return new List<CardBase>();
    }

    public IReadOnlyList<int> GetZoneOptions(ActionContext ctx) {
        return new List<int>();
    }

    public ActionStep GetNextStep(ActionContext ctx) {
        return ActionStep.Execute;
    }
}
