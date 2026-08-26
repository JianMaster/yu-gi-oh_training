using System.Collections.Generic;
using UnityEngine;

public class Player {
    public int ID { get; private set; }
    public int LifePoint { get; private set; } = GameDefines.LIFEPOINT;
    public List<CardBase> Hand { get; private set; } = new();
    public Stack<CardBase> Deck { get; private set; } = new();
    // public List<Card> ExtraDeck { get; private set; }
    public List<CardBase> GY { get; private set; } = new();
    public CardBase[] MonsterZone { get; private set; } = new CardBase[5];
    public CardBase[] SpellTrapZone { get; private set; } = new CardBase[5];
    int _normalSummonCount = GameDefines.SUMMON_NORMAL_COUNT;

    public Player(int id) {
        ID = id;
        for (int i = 0; i < GameDefines.MIN_DECK_COUNT; ++i) {
            Deck.Push(new Card_Monster());
        }
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
        Debug.Log($"Player{ID}, 抽取{count}张，当前手牌{Hand.Count}");
    }

    public void CheckHandLimit() {
        if (Hand.Count > GameDefines.MAX_HAND_COUNT) {
            Debug.Log($"当前手牌{Hand.Count}，执行弃牌处理");
        }
    }

    public List<int> GetAvailableMonsterZone() {
        List<int> zoneIds = new();
        for(int i = 0; i < MonsterZone.Length; ++i) {
            if(MonsterZone[i] == null) {
                zoneIds.Add(i);
            }
        }
        return zoneIds;
    }

    public void NormalSummon(int handId, int zoneId) {
        Debug.Log($"通常召唤怪兽到区域{zoneId}");
    }
}
