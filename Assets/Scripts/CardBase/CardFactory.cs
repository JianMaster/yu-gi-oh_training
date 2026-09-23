using System;
using System.Reflection;
using UnityEngine;

public static class CardFactory {
    static Assembly s_cardAssembly = typeof(CardFactory).Assembly;
    public static CardBase CreateInstance(string cardId, Player player) {
        throw new NotImplementedException();
        Type cardType = s_cardAssembly.GetType($"Card_{cardId}");
        if (cardType == null) {
            Debug.LogError($"找不到 Card_{cardId}");
            return null;
        }
        CardBase card = Activator.CreateInstance(cardType) as CardBase;
        return card;
    }
}