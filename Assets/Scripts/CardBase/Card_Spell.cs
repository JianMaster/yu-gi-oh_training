using System.Collections.Generic;

public class Card_Spell : CardBase {
    public Card_Spell(CardData data, Player player) : base(data, player) {
        _defaultActions = new() {
            ActionType.Activate,
            ActionType.SpellTrapSet,
        };
    }

    public override List<ActionType> GetAction() {
        return _defaultActions;
    }

    public override bool CanActivate(ActionType action, ActionContext context) {
        return true;
    }

    public override void Activate() {
        EffectContext context = new() {
            activater = Owner,
            value = _data.effectValue
        };
        _effect.Resolve(context);
    }

    public override void TurnStart() {

    }

    public override string ShowInfo() {
        return Name;
    }
}