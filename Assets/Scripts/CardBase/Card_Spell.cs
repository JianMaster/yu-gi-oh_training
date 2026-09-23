public class Card_Spell : CardBase {
    IEffect _effect;
    public Card_Spell(CardData data, Player player) : base(data, player) {
        _effect = EffectFactory.CreateInstance(data);
        TurnStart();
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