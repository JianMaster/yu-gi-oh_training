using System;
using UnityEngine;

public class GameController {
    GameState _gameState;
    
    public GameController(int first) {
        _gameState = new(first);
    }
    public void Update(InputData command) {
        if (command.draw) {
            _gameState.TurnOwner.Draw(GameDefines.START_CARD_COUNT);
            _gameState.Opponent.Draw(GameDefines.START_CARD_COUNT);
        }

        

        
    }


}