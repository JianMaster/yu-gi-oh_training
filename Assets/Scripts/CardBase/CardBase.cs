using System.Collections.Generic;

public abstract class CardBase {
    protected CardData _data;
    protected IEffect _effect;
    protected List<ActionType> _defaultActions;
    public Player Owner { get; protected set; }
    public ZoneType ZoneType { get; protected set; }
    public int ZoneId { get; protected set; }
    public CardFace Face { get; protected set; }
    public bool SetTurn { get; private set; }
    public bool SetAndDown => SetTurn && Face == CardFace.FaceDown;

    public string ID { get; protected set; }
    public string Name { get; protected set; }
    public CardType CardType { get; protected set; }
    public IEffect Effect => _effect;
    public bool HasEffect => _effect is not Effect_None;

    public CardBase(CardData data, Player belong) {
        _data = data;
        _effect = EffectFactory.CreateInstance(data);
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
    public virtual List<ActionType> GetAction() {
        return new();
    }
    public virtual void Set() {
        SetTurn = true;
        Face = CardFace.FaceDown;
    }
    public virtual bool CanActivate(ActionType action, ActionContext context) {
        return _effect.CheckCanActive(action, context);
    }
    public virtual void Activate(ref EffectContext context) {
        Face = CardFace.FaceUp;
    }
    public virtual void TurnStart() {
        SetTurn = false;
    }
    public virtual string ShowInfo() {
        return Name;
    }

}
