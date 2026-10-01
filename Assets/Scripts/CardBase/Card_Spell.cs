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

    public override void Set() {
        base.Set();
    }

    public override void Activate(ref EffectContext context) {
        base.Activate(ref context);
        context.value = _data.effectValue;
        _effect.Resolve(context);
    }

    public override void TurnStart() {

    }
}