public enum CardType
{
    Monster,
    Spell,
    Trap
}

public enum CardFace {
    FaceUp,
    FaceDown
}

public enum ZoneType {
    Deck,
    Hand,
    GY,
    Monster,
    SpellTrap
}

public enum Phase
{
    Draw,
    // Stand,
    Main1,
    Battle,
    // Main2,
    End
}

public enum CommandType {
    NormalSummon,
    SpecialSummon,
    MonsterSet,
    MonsetFilp,
    Attack,
    SpellTrapSet,
    SpellTrapActivate
}

public enum EffectType {
    NormalSummon,
    SpecialSummon,
}

public enum MonsterAttribute {
    Light,
    Dark
}
public enum MonsterType {
    Warrior,
    Dragon,
    Spellcaster
}

public enum MonterPosition {
    Attack,
    Defense
}


public enum Command {
    Draw,
    NextPhase,
    NormalSummon,
    Attack,
    Cancel,
    Left,
    Right,
}