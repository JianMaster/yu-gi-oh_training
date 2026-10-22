using System.Collections.Generic;

public class ActionContext {
    public Player player;
    public Player opponent;
    public CardBase selectedCard = null;
    public int toZoneId = 0;
    public CardBase targetCard = null;

    public void Reset() {
        selectedCard = null;
        toZoneId = 0;
        targetCard = null;
    }
}