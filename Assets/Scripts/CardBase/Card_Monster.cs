public class Card_Monster : CardBase {
    public MonsterAttribute Attribute { get; protected set; }
    public MonsterType Type { get; protected set; }
    public MonterPosition Position { get; protected set; }
    public int Level { get; protected set; }
    public int Atk { get; protected set; }
    public int Def { get; protected set; }

    int _attackCount;
    public Card_Monster() : base() {
        CardType = CardType.Monster;
        _attackCount = 1;
    }

    public void NormalSummon() {
        Face = CardFace.FaceUp;
        Position = MonterPosition.Attack;
    }

    public void Set() {
        Face = CardFace.FaceDown;
        Position = MonterPosition.Defense;
    }

    public void Flip() {
        Face = CardFace.FaceUp;
        Position = MonterPosition.Attack;
    }

    public void AttackFlip() {
        Face = CardFace.FaceUp;
    }

    public void ChangePosition() {
        Position = Position == MonterPosition.Attack ? MonterPosition.Defense : MonterPosition.Attack;
    }

    public bool CanAttack() {
        return _attackCount > 0 && Position == MonterPosition.Attack;
    }
    public void Attack(Card_Monster opponent) {
        
    }

}