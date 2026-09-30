using System;
using UnityEngine;

[CreateAssetMenu(fileName = "CardData", menuName = "Scriptable Objects/CardData")]
public class CardData : ScriptableObject {
    public string id;
    public string cardName;
    public CardType cardType;
    public EffectType effectType;
    public int effectValue;

    public int atk;
    public int def;
    public int level;
    public MonsterAttribute attribute;
    public MonsterType type;

}