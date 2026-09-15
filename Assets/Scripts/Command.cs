using UnityEngine;

public class Command {
    public CommandType Type { get; set; } = CommandType.None;
    public Player Excuter { get; set; }
    public CardBase TargetCard { get; set; }
    public int TargetZoneId { get; set; }
    public Command() { }
    public Command(CommandType type) {
        Type = type;
    }
}
