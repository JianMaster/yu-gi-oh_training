public class GameController {
    Phase _curPhase;
    Player _player;
    Player _enemy;
    public GameController() {
        _curPhase = Phase.Draw;
        _player = new();
        _enemy = new();
    }
    public void Update(bool next) {
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

        if (next) {
            _curPhase++;
            if (_curPhase > Phase.End) {
                _curPhase = Phase.Draw;
            }
        }
    }


}