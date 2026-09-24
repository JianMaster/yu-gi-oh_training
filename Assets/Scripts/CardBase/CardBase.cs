
public abstract class CardBase {
    protected CardData _data;
    protected IEffect _effect;
    public Player Belong { get; protected set; }
    public ZoneType ZoneType { get; protected set; }
    public int ZoneId { get; protected set; }
    public string ID { get; protected set; }
    public string Name { get; protected set; }
    public CardType CardType { get; protected set; }
    public CardFace Face { get; protected set; }

    public CardBase(CardData data, Player belong) {
        _data = data;
        _effect = EffectFactory.CreateInstance(data);
        Belong = belong;
        ID = data.id;
        Name = data.name;
        CardType = data.cardType;
    }

    public void ChangeZone(ZoneType zoneType, int zoneId) {
        ZoneType = zoneType;
        ZoneId = zoneId;
    }
    public virtual bool CanActivate() {
        return _effect is not Effect_None;
    }
    public virtual void Activate() { }
    public virtual void TurnStart() { }
    public virtual string ShowInfo() {
        return Name;
    }

}
