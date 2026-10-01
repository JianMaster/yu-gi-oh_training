using System.Collections.Generic;

public class Card_Trap : CardBase { 
    public Card_Trap(CardData data, Player belong) : base(data, belong) {
        _defaultActions = new() {
            ActionType.Activate,
            ActionType.SpellTrapSet,
        };
    }

    public override List<ActionType> GetAction() {
        return _defaultActions;
    }

    public override void TurnStart() {
        base.TurnStart();
    }
}