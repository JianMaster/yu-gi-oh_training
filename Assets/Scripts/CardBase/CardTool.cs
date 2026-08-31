using System;
using System.Reflection;
using UnityEngine;

public static class CardTool {
    static Assembly cardAssembly = typeof(CardTool).Assembly;
    public static CardBase CreateInstance(string cardId) {
        Type cardType = cardAssembly.GetType($"Card_{cardId}");
        if (cardType == null) {
            Debug.LogError($"找不到 Card_{cardId}");
            return null;
        }
        CardBase card = Activator.CreateInstance(cardType) as CardBase;
        return card;
    }
}