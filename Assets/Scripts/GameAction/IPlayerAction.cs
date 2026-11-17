using System;
using System.Collections.Generic;
using UnityEngine;

public interface IPlayerAction {
    ActionType Type { get; }
    bool CanUse(CardBase card, Player player, GameState state);
    ActionStep GetNextStep(ActionContext ctx);
    IReadOnlyList<int> GetZoneOptions(ActionContext ctx);
    IReadOnlyList<CardBase> GetTargetOptions(ActionContext ctx);
    void Execute(ActionContext ctx, DuelAction duelAction);
}
