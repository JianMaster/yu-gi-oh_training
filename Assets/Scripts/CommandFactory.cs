public static class CommandFactory {
    public static Command CreateNone() {
        return new Command();
    }
    public static Command CreateNextPhase(Player player) {
        return new Command() {
            Type = CommandType.NextPhase,
            Excuter = player,
        };
    }
    public static Command CreateNormalSummon(Player player, CardBase card, int zoneId) {
        Command command = new Command_NormalSummon() {
            Excuter = player,
            TargetCard = card,
            TargetZoneId = zoneId,
        };
        return command;
    }

    public static Command CreateAttack(Player player, Player opponent, CardBase attackCard, CardBase targetCard, bool isDirectAttack) {
        Command command = new Command_Attack() {
            Excuter = player,
            Opponent = opponent,
            AttackCard = attackCard,
            TargetCard = targetCard,
            IsDirectAttack = isDirectAttack,
        };
        return command;
    }
}
