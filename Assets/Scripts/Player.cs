using System;
using System.Collections.Generic;
using UnityEngine;

public class Player {
    PlayerData _data;
    Dictionary<ZoneType, List<CardBase>> _zone;
    public int ID { get; private set; }
    public int LifePoint { get; private set; } = GameDefines.LIFEPOINT;
    List<CardBase> _hand = new();
    List<CardBase> _deck = new();
    // List<Card> _extraDeck = new();
    List<CardBase> _GY = new();
    List<CardBase> _monsterZone = new() { null, null, null, null, null };
    List<CardBase> _spellTrapZone = new() { null, null, null, null, null };

    public int NormalSummonCount { get; private set; }

    public Player(int id, PlayerData data) {
        _zone = new() {
            {ZoneType.Deck, _deck},
            {ZoneType.Hand, _hand},
            {ZoneType.GY, _GY},
            {ZoneType.Monster, _monsterZone},
            {ZoneType.SpellTrap, _spellTrapZone},
        };
        ID = id;
        _data = data;
        string[] cardIds = data.Deck.Split(' ');
        foreach (var cardId in cardIds) {
            CardBase card = CardFactory.CreateInstance(cardId, this);
            if (card != null) {
                _deck.Add(card);
                card.ChangeZone(ZoneType.Deck, _deck.Count - 1);
            }
        }

        TurnStart();
    }

    void MoveCard(CardBase card, ZoneType to, int toId = -1) {
        ZoneType from = card.ZoneType;
        int fromId = card.ZoneId;
        List<CardBase> fromZone = _zone[from];
        List<CardBase> toZone = _zone[to];
        bool fromField = from == ZoneType.Monster || from == ZoneType.SpellTrap;
        bool toField = to == ZoneType.Monster || to == ZoneType.SpellTrap;
        if (fromField) {
            _monsterZone[fromId] = null;
        }
        else {
            fromZone.RemoveAt(fromId);
        }

        if (toField) {
            toZone[toId] = card;
        }
        else {
            toZone.Add(card);
        }
        card.ChangeZone(to, toZone.Count - 1);
    }


    public void TurnStart() {
        NormalSummonCount = 0;
        _hand.ForEach(card => card.TurnStart());
        _GY.ForEach(card => card.TurnStart());
        _monsterZone.ForEach(card => card?.TurnStart());
        _spellTrapZone.ForEach(card => card?.TurnStart());
    }

    public void TurnEnd() { }

    public void TakeDamage(int damage) {
        LifePoint -= damage;
        Log($"承受伤害：{damage}, 生命值剩余{LifePoint}");
        if (LifePoint <= 0) {
            Log($"游戏结束");
        }
    }

    public void Heal(int heal) {
        LifePoint += heal;
        Log($"恢复伤害：{heal}, 生命值剩余{LifePoint}");
    }

    public void Draw(int count) {
        for (int i = 0; i < count; ++i) {
            if (_deck.Count == 0) {
                Debug.LogError("No Deck!!");
                break;
            }

            Debug.Log(TextData.Instance.GetFormatText(Text_ID.DrawInfo, _deck[^1].ShowInfo()));
            MoveCard(_deck[^1], ZoneType.Hand);
        }
        Debug.Log(string.Format(TextData.Instance.GetText(Text_ID.Draw), ID, count, _hand.Count));
    }

    public void CheckHandLimit() {
        if (_hand.Count > GameDefines.MAX_HAND_COUNT) {
            Log($"当前手牌{_hand.Count}, 执行弃牌处理");
        }
    }

    public IReadOnlyList<CardBase> GetZoneCards(ZoneType zoneType) {
        return _zone[zoneType];
    }

    public List<int> GetAvailableZone(ZoneType zoneType) {
        List<int> zoneIds = new();
        for (int i = 0; i < _zone[zoneType].Count; ++i) {
            if (_zone[zoneType][i] == null) {
                zoneIds.Add(i);
            }
        }
        Log($"当前可用区域：" + string.Join(" ", zoneIds));
        return zoneIds;
    }

    public void NormalSummon(Card_Monster card, int targetZoneId) {
        Debug.Log(string.Format(TextData.Instance.GetText(Text_ID.NormalSummon), ID, card.Name, targetZoneId));
        NormalSummonCount--;
        MoveCard(card, ZoneType.Monster, targetZoneId);
        card.NormalSummon();
    }

    public List<CardBase> GetAttackTarget() {
        List<CardBase> targets = new();
        for (int i = 0; i < _monsterZone.Count; ++i) {
            if (_monsterZone[i] != null) {
                targets.Add(_monsterZone[i]);
            }
        }

        if (targets.Count == 0) {
            // 没有攻击目标，则攻击玩家
            CardBase player = new Card_Monster(new CardData(), this);
            player.ChangeZone(ZoneType.Monster, GameDefines.PLAYER_ZONE);
            targets.Add(player);
        }

        return targets;
    }
    public void Activate(CardBase card, int targetZoneId, EffectContext context) {
        if (card.Owner != this) {
            Debug.LogError("卡牌不属于该玩家");
            return;
        }
        MoveCard(card, ZoneType.SpellTrap, targetZoneId);
        card.Activate(ref context);
        MoveCard(card, ZoneType.GY);
    }

    public void DestroyCard(CardBase card) {
        MoveCard(card, ZoneType.GY);
    }

    public void Log(string txt) {
        Debug.Log($"Player{ID}:   " + txt);
    }

}
