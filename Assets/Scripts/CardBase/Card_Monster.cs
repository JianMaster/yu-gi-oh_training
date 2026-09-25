using System.Collections.Generic;

public class Card_Monster : CardBase {
    public MonsterAttribute Attribute { get; protected set; }
    public MonsterType Type { get; protected set; }
    public MonterPosition Position { get; protected set; }
    public int Level { get; protected set; }
    public int Atk { get; protected set; }
    public int Def { get; protected set; }

    public int AttackCount { get;private set; }
    public int ChangePositionCount { get; private set; }

    public Card_Monster(CardData data, Player player) : base(data, player) {
        _defaultActions = new() {
            ActionType.NormalSummon,
            ActionType.SpecialSummon,
            ActionType.Activate,
            ActionType.ChangePosition,
            ActionType.MonsterSet,
            ActionType.Attack,
        };
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
        ChangePositionCount--;
        Position = Position == MonterPosition.Attack ? MonterPosition.Defense : MonterPosition.Attack;
    }

    public void BeforeAttack() { }

    public void AfterAttack() {
        AttackCount--;
    }

    public override List<ActionType> GetAction() {
        return _defaultActions;
    }

    public override void CheckAction(in List<ActionType> actions, ActionContext context) {
        
    }

    public override void TurnStart() {
        AttackCount = 0;
        ChangePositionCount = 0;
    }

    public override string ShowInfo() {
        string info = $"名称：{Name}, 攻击力：{Atk}, 防御力：{Def}";
        return info;
    }

}