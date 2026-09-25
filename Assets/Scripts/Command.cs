using UnityEngine;

public class Action {
    public ActionType Type { get; set; } = ActionType.None;
    public Player Excuter { get; set; }
    public Action() { }
    public Action(ActionType type) {
        Type = type;
    }
}

public class Action_NormalSummon : Action {
    public CardBase TargetCard { get; set; }
    public int TargetZoneId { get; set; }
    public Action_NormalSummon() : base(ActionType.NormalSummon) { }
}

public class Action_Attack : Action {
    public Player Opponent { get; set; }
    public Card_Monster AttackMonster { get; set; }
    public Card_Monster TargetMonster { get; set; }
    public bool IsDirectAttack { get; set; }
    public Action_Attack() : base(ActionType.Attack) { }
}

public class Action_Activate : Action {
    public CardBase TargetCard { get; set; }
    public int TargetZoneId { get; set; }
    public Action_Activate() : base(ActionType.Activate) { }
}
