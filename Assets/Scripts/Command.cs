using UnityEngine;

public class Command {
    public CommandType Type { get; private set; }
    public Command(CommandType t) {
        Type = t;
    }
}
