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

    int _normalSummonCount;
    public bool CanNormalSummon => _normalSummonCount > 0;


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
            CardBase card = CardFactory.CreateInstance(cardId);
            if (card != null) {
                _deck.Add(card);
                card.Belong = ID;
                card.ChangeZone(ZoneType.Deck, _deck.Count - 1);
            }
        }

        TurnStart();
    }


    public void TurnStart() {
        _normalSummonCount = GameDefines.SUMMON_NORMAL_COUNT;
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

    public void Draw(int count) {
        for (int i = 0; i < count; ++i) {
            if (_deck.Count == 0) {
                Debug.LogError("No Deck!!");
                break;
            }

            int idx = _deck.Count - 1;
            _deck[idx].ChangeZone(ZoneType.Hand, _hand.Count);
            _hand.Add(_deck[idx]);
            _deck.RemoveAt(idx);
        }
        Debug.Log(string.Format(TextData.Instance.GetText(Text_ID.Draw), ID, count, _hand.Count));
    }

    public bool CheckHand(int id) {
        if (id >= _hand.Count) {
            Log($"手牌选择错误：{id}");
            return false;
        }
        return true;
    }

    public void CheckHandLimit() {
        if (_hand.Count > GameDefines.MAX_HAND_COUNT) {
            Log($"当前手牌{_hand.Count}, 执行弃牌处理");
        }
    }

    public List<CardBase> GetZoneCards(ZoneType zoneType) {
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
        card.NormalSummon();
        card.ChangeZone(ZoneType.Monster, zoneId);
        _normalSummonCount--;
        _monsterZone[zoneId] = _hand[card.ZoneId];
        _hand.RemoveAt(card.ZoneId);
    }

    public bool CheckMonsterCanAttack(int zoneId) {
        if (zoneId < _monsterZone.Count && _monsterZone[zoneId] == null) {
            return false;
        }
        Card_Monster monster = _monsterZone[zoneId] as Card_Monster;
        return monster.CanAttack();
    }

    public List<int> GetAttackTarget() {
        List<int> targets = new();
        for (int i = 0; i < _monsterZone.Count; ++i) {
            if (_monsterZone[i] != null) {
                targets.Add(i);
            }
        }
        // 空场
        if (targets.Count == 0) {
            targets.Add(GameDefines.PLAYER_ZONE);
        }

        Log("当前可攻击对象" + string.Join(" ", targets));

        return targets;
    }

    public void Attack(int self, Player opponent, int target) {
        Card_Monster selfMonster = _monsterZone[self] as Card_Monster;
        if (target == GameDefines.PLAYER_ZONE) {
            Log("直接攻击玩家");
            opponent.TakeDamage(selfMonster.Atk);
            selfMonster.AfterAttack();
            return;
        }

        Card_Monster opponentMonster = opponent._monsterZone[target] as Card_Monster;
        Log($"{selfMonster.Name}攻击 player{opponent.ID}的{opponentMonster.Name}");
        if (opponentMonster.Position == MonterPosition.Attack) {
            int atk1 = selfMonster.Atk, atk2 = opponentMonster.Atk;
            Log($"{selfMonster.Name} 攻击力: {atk1}, player{opponent.ID}: {opponentMonster.Name} 攻击力：{atk2}");
            if (atk1 > atk2) {
                opponent.DestroyCard(ZoneType.Monster, target);
                opponent.TakeDamage(atk1 - atk2);
            }
            else if (atk2 > atk1) {
                DestroyCard(ZoneType.Monster, self);
                TakeDamage(atk2 - atk1);
            }
            else {
                DestroyCard(ZoneType.Monster, self);
                opponent.DestroyCard(ZoneType.Monster, target);
            }
        }
        else {
            int atk = selfMonster.Atk, def = opponentMonster.Def;
            Log($"{selfMonster.Name} 攻击力：{atk}, {opponentMonster.Name} 防御力：{atk}");
            if (atk > def) {
                opponent.DestroyCard(ZoneType.Monster, target);
            }
            else if (def > atk) {
                TakeDamage(def - atk);
            }
        }
        selfMonster.AfterAttack();
    }

    public void DestroyCard(ZoneType from, int idx) {
        List<CardBase> fromzone = _zone[from];
        CardBase card = fromzone[idx];
        fromzone[idx] = null;
        card.ChangeZone(ZoneType.GY, _GY.Count);
        _GY.Add(card);

        Log($"卡牌{card.Name}被破坏");
    }


    public void Log(string txt) {
        Debug.Log($"Player{ID}:   " + txt);
    }
}
