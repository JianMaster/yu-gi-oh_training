using System;
using UnityEngine;

public class GameController {
    Phase _curPhase;
    Player _player1;
    Player _player2;

    int _turn = 1;
    int _first = 0; // 0为player1先手，1为后手
    Player TurnOwner => _turn % 2 + _first == 1 ? _player1 : _player2;
    Player Opponent => _turn % 2 + _first == 0 ? _player1 : _player2;
    
    public GameController(int first) {
        _first = first;
        _curPhase = Phase.Draw;
        _player1 = new();
        _player2 = new();
    }
    public void Update(InputData a) {
        // switch (_curPhase) {
        //     case Phase.Draw:
        //         break;
        //     case Phase.Stand:
        //         break;
        //     case Phase.Main1:
        //         break;
        //     case Phase.Battle:
        //         break;
        //     case Phase.Main2:
        //         break;
        //     case Phase.End:
        //         break;
        // }

        
    }


}