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

            int idx = _deck.Count - 1;
            CardBase card = _deck[idx];
            _hand.Add(card);
            _deck.RemoveAt(idx);
            card.ChangeZone(ZoneType.Hand, _hand.Count - 1);
            Debug.Log(TextData.Instance.GetFormatText(Text_ID.DrawInfo, card.ShowInfo()));
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

    public List<int> GetAvailableMonsterZone() {
        List<int> zoneIds = new();
        for (int i = 0; i < _monsterZone.Count; ++i) {
            if (_monsterZone[i] == null) {
                zoneIds.Add(i);
            }
        }
        Log($"当前可用怪兽区域：" + string.Join(" ", zoneIds));
        return zoneIds;
    }

    public void NormalSummon(Card_Monster card, int zoneId) {
        Debug.Log(string.Format(TextData.Instance.GetText(Text_ID.NormalSummon), ID, card.Name, zoneId));
        NormalSummonCount--;
        _monsterZone[zoneId] = _hand[card.ZoneId];
        _hand.RemoveAt(card.ZoneId);
        card.NormalSummon();
        card.ChangeZone(ZoneType.Monster, zoneId);
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

    public void DestroyCard(CardBase card) {
        _zone[card.ZoneType][card.ZoneId] = null;
        _GY.Add(card);
        card.ChangeZone(ZoneType.GY, _GY.Count - 1);
    }


    public void Log(string txt) {
        Debug.Log($"Player{ID}:   " + txt);
    }
}
