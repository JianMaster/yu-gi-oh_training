using UnityEngine;
using UnityEngine.InputSystem;

public class Main : MonoBehaviour {
    GameController _game;
    GameState _state;
    EventSystem _event;
    InteractionController _interaction;
    [SerializeField] InputManager _inputManager;
    [SerializeField] bool player1_first;
    [SerializeField] PlayerData data1;
    [SerializeField] PlayerData data2;
    InputData _inputData = new();
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        _state = new(player1_first, data1, data2);
        GameRule rule = new(_state);
        ActionQuery query  = new(rule);
        _interaction = new(_state, query);
        _event = new();
        _game = new(_state, _event);

        
        _state.OnNextTurn += _interaction.ChangePlayer;

        _game.GameStart();
    }

    // Update is called once per frame
    void Update() {
        _inputManager.GetInput(ref _inputData);
        Action action = _interaction.GetAction(_inputData);
        _game.ExcuteCommand(action);

    }

    void LateUpdate() {
        _inputData.Reset();
    }
}

