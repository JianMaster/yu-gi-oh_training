using System.Collections.Generic;
using UnityEngine;

public class Player {
    public List<Card> Cards { get; set; } = new();
    public List<Card> Deck { get; set; } = new();
    // public List<Card> ExtraDeck { get; set; }
    public List<Card> GY { get; set; } = new();

    public Player() {

    }
}
