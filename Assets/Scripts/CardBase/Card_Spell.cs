public class Card_Spell : CardBase {
    public Card_Spell(CardData data, Player player) : base(data, player) {
        TurnStart();
    }

    public bool CanActivate() {
        return true;
    }

    public void Activate() {
        EffectContext context = new();

        _effect.Resolve(context);
    }

    public override void TurnStart() {
        
    }

    public override string ShowInfo() {
        return Name;
    }
}