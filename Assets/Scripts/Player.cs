using System.Collections.Generic;
using UnityEngine;

public class Player {
    public int ID { get; set; }
    public int LifePoint { get; set; } = GameDefines.LIFEPOINT;
    public List<CardBase> Hand { get; set; } = new();
    public Stack<CardBase> Deck { get; set; } = new();
    // public List<Card> ExtraDeck { get; set; }
    public List<CardBase> GY { get; set; } = new();
    public List<CardBase> MonsterZone { get; set; } = new();
    public List<CardBase> SpellTrapZone { get; set; } = new();
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
}
