using UnityEngine;

public static class GameLog {
    public static void Log(this Player player, string txt) {
        Debug.Log($"Player{player.ID}" + txt);
    }
}