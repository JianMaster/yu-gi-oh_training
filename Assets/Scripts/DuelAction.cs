using UnityEngine;

public class DuelAction {
    public void Draw(Player player, int count) {
        var deck = player.GetZone(ZoneType.Deck);
        var hand = player.GetZone(ZoneType.Hand);
        for (int i = 0; i < count; ++i) {
            if (deck.Count == 0) {
                Debug.LogError("No Deck!!");
                break;
            }

            Debug.Log(TextData.Instance.GetFormatText(Text_ID.DrawInfo, deck[^1].ShowInfo()));
            player.MoveCard(deck[^1], ZoneType.Hand);
        }
        Debug.Log(string.Format(TextData.Instance.GetText(Text_ID.Draw), player.ID, count, hand.Count));
    }

    public void Destroy(CardBase card) {
        var player = card.Owner;
        player.MoveCard(card, ZoneType.GY);

        Debug.Log(TextData.Instance.GetFormatText(Text_ID.Destroy, player.ID, card.Name));
    }

    public void Damage(Player player, int value) {
        
    }

    public void Heal(Player player, int value) {
        
    }
}
