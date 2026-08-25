public class GameState {
    Player _player1;
    Player _player2;
    int _first = 0; // 0为player1先手，1为后手
    
    public Phase CurPhase { get; set; }
    public int Turn { get; set; } = 1;
    public Player TurnOwner => Turn % 2 + _first == 1 ? _player1 : _player2;
    public Player Opponent => Turn % 2 + _first == 0 ? _player1 : _player2;

    public GameState(int first) {
        CurPhase = Phase.Draw;
        _first = first;
        _player1 = new();
        _player2 = new();
    }
}