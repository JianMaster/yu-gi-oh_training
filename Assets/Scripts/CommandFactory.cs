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

    public static Command CreateAttack(Player player, Player opponent, Card_Monster attackMonster, Card_Monster targetMonster, bool isDirectAttack) {
        Command command = new Command_Attack() {
            Excuter = player,
            Opponent = opponent,
            AttackMonster = attackMonster,
            TargetMonster = targetMonster,
            IsDirectAttack = isDirectAttack,
        };
        return command;
    }

    public static Command CreateActivate(Player player, CardBase targetCard, int targetZoneId) {
        Command command = new Command_Activate() {
            Excuter = player,
            TargetCard = targetCard,
            TargetZoneId = targetZoneId,
        };
        return command;
    }
}
