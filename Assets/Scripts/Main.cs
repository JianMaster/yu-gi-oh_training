using UnityEngine;
using UnityEngine.InputSystem;

public class Main : MonoBehaviour {
    GameController _game;
    [SerializeField] InputActionReference _space;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        _game = new();
    }

    // Update is called once per frame
    void Update() {
        _game.Update(_space.action.WasPressedThisFrame());
    }
}
