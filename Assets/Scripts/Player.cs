using System;
using System.Collections.Generic;
using UnityEngine;

public class Player {
    PlayerData _data;
    Dictionary<ZoneType, List<CardBase>> _zone;
    public int ID { get; private set; }
    public int LifePoint { get; private set; } = GameDefines.LIFEPOINT;
    public List<CardBase> Hand { get; private set; } = new();
    public List<CardBase> Deck { get; private set; } = new();
    // public List<Card> ExtraDeck { get; private set; }
    public List<CardBase> GY { get; private set; } = new();
    public List<CardBase> MonsterZone { get; private set; } = new() { null, null, null, null, null };
    public List<CardBase> SpellTrapZone { get; private set; } = new() { null, null, null, null, null };

    int _normalSummonCount;
    public bool CanNormalSummon => _normalSummonCount > 0;


    public Player(int id, PlayerData data) {
        _zone = new() {
            {ZoneType.Deck, Deck},
            {ZoneType.Hand, Hand},
            {ZoneType.GY, GY},
            {ZoneType.Monster, MonsterZone},
            {ZoneType.SpellTrap, SpellTrapZone},
        };
        ID = id;
        _data = data;
        string[] cardIds = data.Deck.Split(' ');
        foreach (var cardId in cardIds) {
            CardBase card = CardTool.CreateInstance(cardId);
            if (card != null) {
                Deck.Add(card);
                card.Belong = ID;
            }
        }

        TurnStart();
    }


    public void TurnStart() {
        _normalSummonCount = GameDefines.SUMMON_NORMAL_COUNT;
        Hand.ForEach(card => card.TurnStart());
        GY.ForEach(card => card.TurnStart());
        MonsterZone.ForEach(card => card?.TurnStart());
        SpellTrapZone.ForEach(card => card?.TurnStart());
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
            if (Deck.Count == 0) {
                Debug.LogError("No Deck!!");
                break;
            }

            int idx = Deck.Count - 1;
            Hand.Add(Deck[idx]);
            Deck.RemoveAt(idx);
        }
        Log($"抽取{count}张, 当前手牌{Hand.Count}");
    }

    public bool CheckHand(int id) {
        if (id >= Hand.Count) {
            Log($"手牌选择错误：{id}");
            return false;
        }
        return true;
    }

    public void CheckHandLimit() {
        if (Hand.Count > GameDefines.MAX_HAND_COUNT) {
            Log($"当前手牌{Hand.Count}, 执行弃牌处理");
        }
    }

    public List<int> GetAvailableMonsterZone() {
        List<int> zoneIds = new();
        for (int i = 0; i < MonsterZone.Count; ++i) {
            if (MonsterZone[i] == null) {
                zoneIds.Add(i);
            }
        }
        Log($"当前可用怪兽区域：" + string.Join(" ", zoneIds));
        return zoneIds;
    }

    public void NormalSummon(int selectHand, int zoneId) {
        Log($"通常召唤怪兽{Hand[selectHand].Name}到区域{zoneId}");
        Card_Monster card = Hand[selectHand] as Card_Monster;
        card.NormalSummon();
        _normalSummonCount--;
        MonsterZone[zoneId] = Hand[selectHand];
        Hand.RemoveAt(selectHand);
    }

    public bool CheckMonsterCanAttack(int zoneId) {
        if (zoneId < MonsterZone.Count && MonsterZone[zoneId] == null) {
            return false;
        }
        Card_Monster monster = MonsterZone[zoneId] as Card_Monster;
        return monster.CanAttack();
    }

    public List<int> GetAttackTarget() {
        List<int> targets = new();
        for (int i = 0; i < MonsterZone.Count; ++i) {
            if (MonsterZone[i] != null) {
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
        Card_Monster selfMonster = MonsterZone[self] as Card_Monster;
        if (target == GameDefines.PLAYER_ZONE) {
            Log("直接攻击玩家");
            opponent.TakeDamage(selfMonster.Atk);
            selfMonster.AfterAttack();
            return;
        }

        Card_Monster opponentMonster = opponent.MonsterZone[target] as Card_Monster;
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
        GY.Add(card);

        Log($"卡牌{card.Name}被破坏");
    }


    public void Log(string txt) {
        Debug.Log($"Player{ID}:   " + txt);
    }
}
