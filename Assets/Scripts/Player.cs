using System.Collections.Generic;
using UnityEngine;

public class Player {
    public int LifePoint { get; set; } = GameDefines.LIFEPOINT;
    public List<Card> Cards { get; set; } = new();
    public List<Card> Deck { get; set; } = new();
    // public List<Card> ExtraDeck { get; set; }
    public List<Card> GY { get; set; } = new();
    public List<Card> MonsterZone { get; set; } = new();
    public List<Card> SpellTrapZone { get; set; } = new();

    public Player() {
        LifePoint = GameDefines.LIFEPOINT;
    }
}
