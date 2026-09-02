using System;
using System.Collections.Generic;
using UnityEngine;

public class Player {
    PlayerData _data;
    public int ID { get; private set; }
    public int LifePoint { get; private set; } = GameDefines.LIFEPOINT;
    public List<CardBase> Hand { get; private set; } = new();
    public Stack<CardBase> Deck { get; private set; } = new();
    // public List<Card> ExtraDeck { get; private set; }
    public List<CardBase> GY { get; private set; } = new();
    public CardBase[] MonsterZone { get; private set; } = new CardBase[5];
    public CardBase[] SpellTrapZone { get; private set; } = new CardBase[5];

    int _normalSummonCount;
    public bool CanNormalSummon => _normalSummonCount >= GameDefines.SUMMON_NORMAL_COUNT;


    public Player(int id, PlayerData data) {
        ID = id;
        _data = data;
        string[] cardIds = data.Deck.Split(' ');

        foreach (var cardId in cardIds) {
            CardBase card = CardTool.CreateInstance(cardId);
            if (card != null) {
                Deck.Push(card);
                card.Belong = ID;
            }
        }

        TurnStart();
    }


    public void TurnStart() {
        _normalSummonCount = 0;
    }

    public void Draw(int count) {
        for (int i = 0; i < count; ++i) {
            if (Deck.TryPop(out var result)) {
                Hand.Add(result);
            }
            else {
                Debug.LogError("No Deck!!");
            }
        }
        Log($"抽取{count}张，当前手牌{Hand.Count}");
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
            Log($"当前手牌{Hand.Count}，执行弃牌处理");
        }
    }

    public List<int> GetAvailableMonsterZone() {
        List<int> zoneIds = new();
        for (int i = 0; i < MonsterZone.Length; ++i) {
            if (MonsterZone[i] == null) {
                zoneIds.Add(i);
            }
        }
        Log($"当前可用怪兽区域：" + string.Join(" ", zoneIds));
        return zoneIds;
    }

    public void NormalSummon(int selectHand, int zoneId) {
        Log($"通常召唤怪兽{Hand[selectHand].Name}到区域{zoneId}");
        _normalSummonCount++;
        MonsterZone[zoneId] = Hand[selectHand];
        Hand.RemoveAt(selectHand);
    }

    public bool CheckMonsterCanAttack(int zoneId) {
        if (MonsterZone[zoneId] == null) {
            return false;
        }
        return true;
    }

    public List<int> GetAttackTarget() {
        List<int> targets = new();
        for (int i = 0; i < MonsterZone.Length; ++i) {
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
        Card_Monster opponentMonster = opponent.MonsterZone[target] as Card_Monster;
        Log($"{selfMonster.Name}攻击 player{opponent.ID}的{opponentMonster.Name}");
        if (selfMonster.Atk > opponentMonster.Def) {
        }
    }


    public void Log(string txt) {
        Debug.Log($"Player{ID}:   " + txt);
    }
}
