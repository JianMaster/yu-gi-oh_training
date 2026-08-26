using UnityEngine;
using UnityEngine.InputSystem;

public class Main : MonoBehaviour {
    GameController _game;
    [SerializeField] InputManager _inputManager;
    InputData _inputData = new();
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        _game = new(0);
        _game.GameStart();
    }

    // Update is called once per frame
    void Update() {
        _inputManager.GetInput(ref _inputData);
        _game.Update(_inputData);
    }

    void LateUpdate() {
        _inputData.Reset();
    }
}

