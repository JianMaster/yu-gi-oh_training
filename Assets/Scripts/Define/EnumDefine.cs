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

public enum InputType {
    Confirm,
    Cancel,
    Left,
    Right,
    Up,
    Down,
}

public enum CommandType {
    None,
    NormalSummon,
    SpecialSummon,
    MonsterSet,
    MonsetFilp,
    Attack,
    ChangePosition,
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