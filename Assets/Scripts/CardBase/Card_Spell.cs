using System.Collections.Generic;

public class Card_Spell : CardBase {
    public Card_Spell(CardData data, Player player) : base(data, player) {

    }

    public override List<ActionType> GetAction() {
        return new() {
            ActionType.Activate,
            ActionType.SpellTrapSet,
        };
    }

    public override bool CanActivate() {
        return true;
    }

    public override void Activate() {
        EffectContext context = new();

        _effect.Resolve(context);
    }

    public override void TurnStart() {

    }

    public override string ShowInfo() {
        return Name;
    }
}