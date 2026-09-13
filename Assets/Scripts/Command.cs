using UnityEngine;

public class Command {
    public CommandType Type { get; set; }
    public Player Excuter { get; set; }
    public int TargetId { get; set; }
    public int TargetZoneId { get; set; }
    public Command(CommandType t = CommandType.None) {
        Type = t;
    }
}
