
public abstract class CardBase {
    public int Belong { get; set; }
    public ZoneType ZoneType { get; protected set; }
    public int ZoneId { get; protected set; }
    public string ID { get; protected set; }
    public string Name { get; protected set; }
    public CardType CardType { get; protected set; }
    public CardFace Face { get; protected set; }

    public void ChangeZone(ZoneType zoneType, int zoneId) {
        ZoneType = zoneType;
        ZoneId = zoneId;
    }
    public abstract void TurnStart();
    public abstract string ShowInfo();

}
