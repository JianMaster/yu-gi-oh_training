public class Card_Monster : CardBase {
    public MonsterAttribute Attribute { get; protected set; }
    public MonsterType Type { get; protected set; }
    public int Level { get; protected set; }
    public int Atk { get; protected set; }
    public int Def { get; protected set; }
    public Card_Monster() : base() {
        CardType = CardType.Monster;
    }
}