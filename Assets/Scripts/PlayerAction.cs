using System.Collections.Generic;
using UnityEngine;

public interface IPlayerAction {
    ActionType Type { get; }
    bool CanUse(CardBase card, Player player, GameState state);
    ActionStep GetNextStep(ActionContext ctx);
    IReadOnlyList<int> GetZoneOptions(ActionContext ctx);
    IReadOnlyList<CardBase> GetTargetOptions(ActionContext ctx);
    void Execute(ActionContext ctx);
}

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

    public void Execute(ActionContext ctx) {
        var card = ctx.selectedCard as Card_Monster;
        var player = ctx.player;
        var toZoneId = ctx.toZoneId;
        player.OnNormalSummon();
        card.OnNormalSummon();
        player.MoveCard(card, ZoneType.Monster, toZoneId);
        Debug.Log(string.Format(TextData.Instance.GetText(Text_ID.NormalSummon), player.ID, card.Name, toZoneId));
    }

}

public class Action_Attack : IPlayerAction {
    public ActionType Type { get; } = ActionType.Attack;
    public Player Opponent { get; set; }
    public Card_Monster AttackMonster { get; set; }
    public Card_Monster TargetMonster { get; set; }
    public bool IsDirectAttack { get; set; }

    public bool CanUse(CardBase card, Player player, GameState state) {
        Phase phase = state.CurPhase;
        Card_Monster monster = card as Card_Monster;
        if (phase == Phase.Battle &&
            monster.ZoneType == ZoneType.Monster &&
            monster.Position == MonterPosition.Attack &&
            monster.AttackCount < GameDefines.TURN_ATTACK_COUNT) {
            return true;
        }
        return false;
    }

    public void Execute(ActionContext ctx) {
        throw new System.NotImplementedException();
    }

    public IReadOnlyList<CardBase> GetTargetOptions(ActionContext ctx) {
        return ctx.opponent.GetAttackTarget();
    }

    public IReadOnlyList<int> GetZoneOptions(ActionContext ctx) {
        return new List<int>();
    }

    public ActionStep GetNextStep(ActionContext ctx) {
        return ActionStep.Target;
    }
}

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

    public void Execute(ActionContext ctx) {
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
