using System.Collections.Generic;

public class Card_Spell : CardBase {
    public Card_Spell(CardData data, Player player) : base(data, player) {

    }

    public override void Set() {
        Face = CardFace.FaceUp;
        SetTurn = true;
    }

    public override void Activate(ref EffectContext context) {
        Face = CardFace.FaceUp;
        context.value = _data.effectValue;
        _effect.Resolve(context);
    }

    public override void TurnStart() {
        SetTurn = false;
    }
}