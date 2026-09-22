using UnityEngine;

public class Command {
    public CommandType Type { get; set; } = CommandType.None;
    public Player Excuter { get; set; }
    public Command() { }
    public Command(CommandType type) {
        Type = type;
    }
}

public class Command_NormalSummon : Command {
    public CardBase TargetCard { get; set; }
    public int TargetZoneId { get; set; }
    public Command_NormalSummon() : base(CommandType.NormalSummon) { }
}

public class Command_Attack : Command {
    public Player Opponent { get; set; }
    public Card_Monster AttackMonster { get; set; }
    public Card_Monster TargetMonster { get; set; }
    public bool IsDirectAttack { get; set; }
    public Command_Attack() : base(CommandType.Attack) {}
}
