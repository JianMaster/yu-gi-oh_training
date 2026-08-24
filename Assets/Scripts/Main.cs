using UnityEngine;
using UnityEngine.InputSystem;

public class Main : MonoBehaviour {
    GameController _game;
    [SerializeField] InputManager _inputManager;
    InputData inputData = new();
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        _game = new(0);
    }

    // Update is called once per frame
    void Update() {
        _inputManager.GetInput(ref inputData);
        _game.Update(inputData);
    }
}

