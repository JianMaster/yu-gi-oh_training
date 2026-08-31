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

    int _selectHand;
    public bool IsSelectHand => _selectHand != -1;

    public Player(int id, PlayerData data) {
        ID = id;
        _data = data;
        string[] cardIds = data.Deck.Split(' ');

        foreach (var cardId in cardIds) {
            CardBase card = CardTool.CreateInstance(cardId);
            if (card != null)
                Deck.Push(card);
        }

        TurnStart();
    }


    public void TurnStart() {
        _selectHand = -1;
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

    public void CheckHandLimit() {
        if (Hand.Count > GameDefines.MAX_HAND_COUNT) {
            Log($"当前手牌{Hand.Count}，执行弃牌处理");
        }
    }

    public void SetSelectHand(int id) {
        if (id >= Hand.Count) {
            return;
        }
        Log($"选择手牌：{id}");
        _selectHand = id;
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

    public void NormalSummon(int zoneId) {
        Log($"通常召唤怪兽{Hand[_selectHand].Name}到区域{zoneId}");
        _normalSummonCount++;
        MonsterZone[zoneId] = Hand[_selectHand];
        Hand.RemoveAt(_selectHand);
    }


    public void Log(string txt) {
        Debug.Log($"Player{ID}:   " + txt);
    }
}
