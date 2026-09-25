public static class CommandFactory {
    public static Action CreateNone() {
        return new Action();
    }
    public static Action CreateNextPhase(Player player) {
        return new Action() {
            Type = ActionType.NextPhase,
            Excuter = player,
        };
    }
    public static Action CreateNormalSummon(Player player, CardBase card, int zoneId) {
        Action action = new Action_NormalSummon() {
            Excuter = player,
            TargetCard = card,
            TargetZoneId = zoneId,
        };
        return action;
    }

    public static Action CreateAttack(Player player, Player opponent, Card_Monster attackMonster, Card_Monster targetMonster, bool isDirectAttack) {
        Action action = new Action_Attack() {
            Excuter = player,
            Opponent = opponent,
            AttackMonster = attackMonster,
            TargetMonster = targetMonster,
            IsDirectAttack = isDirectAttack,
        };
        return action;
    }

    public static Action CreateActivate(Player player, CardBase targetCard, int targetZoneId) {
        Action action = new Action_Activate() {
            Excuter = player,
            TargetCard = targetCard,
            TargetZoneId = targetZoneId,
        };
        return action;
    }
}
