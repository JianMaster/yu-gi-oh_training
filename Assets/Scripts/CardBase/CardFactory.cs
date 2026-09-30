using UnityEngine;

public static class CardFactory {
    public static CardBase CreateInstance(string cardId, Player player) {
        CardData cardData = Resources.Load<CardData>($"{cardId}");
        CardBase card = null;
        if (cardData.cardType == CardType.Monster) {
            card = new Card_Monster(cardData, player);
        }
        else if (cardData.cardType == CardType.Spell) {
            card = new Card_Spell(cardData, player);
        }
        else if (cardData.cardType == CardType.Trap) {
            card = new Card_Trap(cardData, player);
        }

        return card;
    }
}