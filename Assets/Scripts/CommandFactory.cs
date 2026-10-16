public static class CommandFactory {
    public static Action CreateAction(InteractionContext context) {
        return context.selectedAction switch {
            ActionType.None => CreateNone(),
            ActionType.NextPhase => CreateNextPhase(context.player),
            ActionType.NormalSummon => CreateNormalSummon(context.player, context.selectedCard, context.toZoneId),
            ActionType.Attack => CreateAttack(context.player, context.opponent, context.selectedCard as Card_Monster, context.targetCard as Card_Monster, context.targetCard.ZoneId == GameDefines.PLAYER_ZONE),
            ActionType.Activate => CreateActivate(context.player, context.selectedCard, context.toZone, context.toZoneId),
            _ => CreateNone(),
        };
    }

    static Action CreateNone() {
        return new Action();
    }
    public static Action CreateNextPhase(Player player) {
        return new Action() {
            Type = ActionType.NextPhase,
            Executer = player,
        };
    }
    static Action CreateNormalSummon(Player player, CardBase card, int zoneId) {
        Action action = new Action_NormalSummon() {
            Executer = player,
            SelectCard = card,
            SelectZoneId = zoneId,
        };
        return action;
    }

    static Action CreateAttack(Player player, Player opponent, Card_Monster attackMonster, Card_Monster targetMonster, bool isDirectAttack) {
        Action action = new Action_Attack() {
            Executer = player,
            Opponent = opponent,
            AttackMonster = attackMonster,
            TargetMonster = targetMonster,
            IsDirectAttack = isDirectAttack,
        };
        return action;
    }

    static Action CreateActivate(Player player, CardBase targetCard, ZoneType targetZone, int targetZoneId) {
        Action action = new Action_Activate() {
            Executer = player,
            SelectCard = targetCard,
            toZone = targetZone,
            toZoneId = targetZoneId,
        };
        return action;
    }
}
