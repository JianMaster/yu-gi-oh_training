public enum CardType
{
    Monster,
    Spell,
    Trap
}

public enum Phase
{
    Draw,
    // Stand,
    Main1,
    // Battle,
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