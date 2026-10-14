using System.Collections.Generic;

public class Card_Spell : CardBase {
    public Card_Spell(CardData data, Player player) : base(data, player) {
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