using System.Collections.Generic;
public struct Zone {
    public static Zone Default = new() { type = ZoneType.None, slot = -1 };
    public ZoneType type;
    public int slot;
    public bool IsValid() {
        return type == ZoneType.None;
    }
    public bool IsSlotDefalut() {
        return slot == -1;
    }
}

public abstract class CardBase {
    protected CardData _data;
    protected IEffect _effect;

    public Player Owner { get; protected set; }
    public Zone Zone { get; protected set; }
    public CardFace Face { get; protected set; }
    public bool SetTurn { get; protected set; }
    public bool SetAndDown => SetTurn && Face == CardFace.FaceDown;

    public string ID { get; protected set; }
    public string Name { get; protected set; }
    public CardType CardType { get; protected set; }
    public CardData Data => _data;
    public IEffect Effect => _effect;
    public bool HasEffect => _effect is not Effect_None;

    public CardBase(CardData data, Player belong) {
        _data = data;
        Owner = belong;
        ID = data.id;
        Name = data.cardName;
        CardType = data.cardType;
    }

    public void InitEffect(IEffect effect) {
        if (_effect != null) {
            return;
        }
        _effect = effect;
    }

    public void ChangeZone(ZoneType type, int slot) {
        Zone = new Zone() {
            type = type,
            slot = slot
        };
    }

    public abstract void Set();
    public abstract void Flip();
    public abstract void TurnStart();
    public virtual string ShowInfo() {
        return Name;
    }

}
