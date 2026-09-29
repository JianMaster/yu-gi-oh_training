public static class CommandFactory {
    public static Action CreateAction(InteractionContext context) {
        return context.selectedAction switch {
            ActionType.None => CreateNone(),
            ActionType.NextPhase => CreateNextPhase(context.player),
            ActionType.NormalSummon => CreateNormalSummon(context.player, context.selectedCard, context.selectZoneId),
            ActionType.Attack => CreateAttack(context.player, context.opponent, context.selectedCard as Card_Monster, context.targetCard as Card_Monster, context.targetCard.ZoneId == GameDefines.PLAYER_ZONE),
            ActionType.Activate => CreateActivate(context.player, context.selectedCard, context.selectZoneId),
            _ => CreateNone(),
        };
    }

    static Action CreateNone() {
        return new Action();
    }
    public static Action CreateNextPhase(Player player) {
        return new Action() {
            Type = ActionType.NextPhase,
            Excuter = player,
        };
    }
    static Action CreateNormalSummon(Player player, CardBase card, int zoneId) {
        Action action = new Action_NormalSummon() {
            Excuter = player,
            SelectCard = card,
            SelectZoneId = zoneId,
        };
        return action;
    }

    static Action CreateAttack(Player player, Player opponent, Card_Monster attackMonster, Card_Monster targetMonster, bool isDirectAttack) {
        Action action = new Action_Attack() {
            Excuter = player,
            Opponent = opponent,
            AttackMonster = attackMonster,
            TargetMonster = targetMonster,
            IsDirectAttack = isDirectAttack,
        };
        return action;
    }

    static Action CreateActivate(Player player, CardBase targetCard, int targetZoneId) {
        Action action = new Action_Activate() {
            Excuter = player,
            SelectCard = targetCard,
            SelectZoneId = targetZoneId,
        };
        return action;
    }
}
