using UnityEngine;

public class GameState {
    Player _player1;
    Player _player2;
    bool _first; // true为player1先手，false为后手

    public Phase CurPhase { get; private set; }
    public int Turn { get; private set; } = 1;
    public Player TurnOwner => Turn % 2 == 1 && _first ? _player1 : _player2;
    public Player Opponent => TurnOwner == _player1 ? _player2 : _player1;

    public GameState(bool first) {
        CurPhase = Phase.Draw;

        _first = first;
        _player1 = new(1);
        _player2 = new(2);
    }


    public void NextPhase() {
        if (CurPhase == Phase.End) {
            Turn++;
            CurPhase = Phase.Draw;
        }
        else {
            CurPhase++;
        }

        Debug.Log($"当前回合{Turn}, 当前阶段{CurPhase}");
    }
}