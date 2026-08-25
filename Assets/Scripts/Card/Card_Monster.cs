public class Card_Monster : CardBase {
    public MonsterAttribute Attribute{get;set;}
    public int Level { get; set; }
    public int Atk { get; set; }
    public int Def { get; set; }
    public Card_Monster() : base() {
        CardType = CardType.Monster;
    }
}