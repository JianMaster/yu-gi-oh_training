
public abstract class CardBase {
    CardData _data;
    public Player Belong { get; protected set; }
    public ZoneType ZoneType { get; protected set; }
    public int ZoneId { get; protected set; }
    public string ID { get; protected set; }
    public string Name { get; protected set; }
    public CardType CardType { get; protected set; }
    public CardFace Face { get; protected set; }

    public CardBase(CardData data, Player belong) {
        _data = data;
        Belong = belong;
        ID = data.id;
        Name = data.name;
        CardType = data.cardType;
    }

    public void ChangeZone(ZoneType zoneType, int zoneId) {
        ZoneType = zoneType;
        ZoneId = zoneId;
    }
    public abstract void TurnStart();
    public abstract string ShowInfo();

}
