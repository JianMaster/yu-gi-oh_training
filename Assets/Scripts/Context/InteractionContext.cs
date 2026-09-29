using System.Collections.Generic;

public class InteractionContext {
    public ZoneType curZone = ZoneType.Hand;
    public List<ActionType> actions = null;
    public List<int> zoneIds = null;
    public List<CardBase> targets = null;

    public ActionType selectedAction = ActionType.None;
    public Player player;
    public Player opponent;
    public CardBase selectedCard = null;
    public int selectZoneId = 0;
    public CardBase targetCard = null;

    public void Reset() {
        curZone = ZoneType.Hand;
        actions = null;
        zoneIds = null;
        targets = null;
        
        selectedAction = ActionType.None;
        selectedCard = null;
        selectZoneId = 0;
        targetCard = null;
    }
}