using UnityEngine;
using UnityEngine.InputSystem;

public class Main : MonoBehaviour {
    GameController _game;
    GameState _state;
    InteractionController _interaction;
    [SerializeField] InputManager _inputManager;
    [SerializeField] bool player1_first;
    [SerializeField] PlayerData data1;
    [SerializeField] PlayerData data2;
    InputData _inputData = new();
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        _state = new(player1_first, data1, data2);
        _interaction = new(_state);
        _game = new();
        _game.OnNextPhase += _state.NextPhase;
        _game.OnEndPhase += _interaction.ChangePlayer;

        _game.GameStart();
    }

    // Update is called once per frame
    void Update() {
        _inputManager.GetInput(ref _inputData);
        Command command = _interaction.GetCommand(_inputData);
        _game.ExcuteCommand(command);

    }

    void LateUpdate() {
        _inputData.Reset();
    }
}

