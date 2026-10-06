using System.Collections.Generic;

public class Card_Trap : CardBase { 
    public Card_Trap(CardData data, Player belong) : base(data, belong) {
        
    }

    public override void Activate(ref EffectContext context) {
        Face = CardFace.FaceUp;
        context.value = _data.effectValue;
        _effect.Resolve(context);
    }


    public override void Set() {
        Face = CardFace.FaceDown;
        SetTurn = true;
    }

    public override void TurnStart() {
        SetTurn = false;
    }
}