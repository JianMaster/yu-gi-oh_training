using System.Collections.Generic;

public class ActionContext {
    public bool nextPhase = false;
    public Player player;
    public Player opponent;
    public CardBase selectedCard = null;
    public int toZoneId = 0;
    public CardBase targetCard = null;

    public ActionContext(Player player, Player opponent) {
        this.player = player;
        this.opponent = opponent;
    }

    public void Reset() {
        selectedCard = null;
        toZoneId = 0;
        targetCard = null;
    }
}