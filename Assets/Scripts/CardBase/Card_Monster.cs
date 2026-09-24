using System.Collections.Generic;

public class Card_Monster : CardBase {
    public MonsterAttribute Attribute { get; protected set; }
    public MonsterType Type { get; protected set; }
    public MonterPosition Position { get; protected set; }
    public int Level { get; protected set; }
    public int Atk { get; protected set; }
    public int Def { get; protected set; }

    int _attackCount;
    int _changePositionCount;

    public Card_Monster(CardData data, Player player) : base(data, player) {

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

    public bool CanChangePosition() {
        return _changePositionCount > 0;
    }
    public void ChangePosition() {
        _changePositionCount--;
        Position = Position == MonterPosition.Attack ? MonterPosition.Defense : MonterPosition.Attack;
    }

    public bool CanAttack() {
        return _attackCount > 0 && Position == MonterPosition.Attack;
    }

    public void BeforeAttack() { }

    public void AfterAttack() {
        _attackCount--;
    }

    public override List<CommandType> GetAction() {
        return new() {
            CommandType.NormalSummon,
            CommandType.SpecialSummon,
            CommandType.Activate,
            CommandType.ChangePosition,
            CommandType.MonsterSet,
            CommandType.Attack,
        };
    }

    public override void TurnStart() {
        _attackCount = 1;
        _changePositionCount = 1;
    }

    public override string ShowInfo() {
        string info = $"名称：{Name}, 攻击力：{Atk}, 防御力：{Def}";
        return info;
    }

}