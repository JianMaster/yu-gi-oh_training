using System.Collections.Generic;

public class Card_Trap : CardBase { 
    public Card_Trap(CardData data, Player belong) : base(data, belong) {
        TurnStart();
    }

    public override void Set() {
        Face = CardFace.FaceDown;
        SetTurn = true;
    }
    
    public override void Flip() {
        Face = CardFace.FaceUp;
    }

    public override void TurnStart() {
        SetTurn = false;
    }
}