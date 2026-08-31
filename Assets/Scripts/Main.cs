using UnityEngine;
using UnityEngine.InputSystem;

public class Main : MonoBehaviour {
    GameController _game;
    [SerializeField] InputManager _inputManager;
    [SerializeField] bool player1_first;
    [SerializeField] PlayerData data1;
    [SerializeField] PlayerData data2;
    InputData _inputData = new();
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        _game = new(player1_first, data1, data2);
        _game.GameStart();
    }

    // Update is called once per frame
    void Update() {
        _inputManager.GetInput(ref _inputData);
        _game.ExcuteCommand(_inputData);

    }

    void LateUpdate() {
        _inputData.Reset();
    }
}

