using System.Collections.Generic;

public abstract class CardBase {
    protected CardData _data;
    protected IEffect _effect;
    
    public Player Owner { get; protected set; }
    public ZoneType ZoneType { get; protected set; }
    public int ZoneId { get; protected set; }
    public CardFace Face { get; protected set; }
    public bool SetTurn { get; protected set; }
    public bool SetAndDown => SetTurn && Face == CardFace.FaceDown;

    public string ID { get; protected set; }
    public string Name { get; protected set; }
    public CardType CardType { get; protected set; }
    public IEffect Effect => _effect;
    public bool HasEffect => _effect is not Effect_None;

    public CardBase(CardData data, Player belong) {
        _data = data;
        _effect = EffectFactory.CreateInstance(this, data);
        Owner = belong;
        ID = data.id;
        Name = data.cardName;
        CardType = data.cardType;
        TurnStart();
    }

    public void ChangeZone(ZoneType zoneType, int zoneId) {
        ZoneType = zoneType;
        ZoneId = zoneId;
    }

    public abstract void Set();
    public abstract void Activate(ref EffectContext context);
    public abstract void TurnStart();
    public virtual string ShowInfo() {
        return Name;
    }

}
