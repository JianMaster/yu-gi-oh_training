using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour {
    [SerializeField] InputActionReference _drawAction;
    [SerializeField] InputActionReference _nextAction;

    void OnEnable() {
        _drawAction.action.Enable();
        _nextAction.action.Enable();
    }
    void OnDisable() {
        _drawAction.action.Disable();
        _nextAction.action.Disable();
    }

    public void GetInput(ref InputData inputData) {
        if (_drawAction.action.WasPressedThisFrame()) {
            inputData.draw = true;
        }
        if (_nextAction.action.WasPressedThisFrame()) {
            inputData.nextPhase = true;
        }

    }
}

public class InputData {
    public bool draw;
    public bool nextPhase;

    public void Reset() {
        draw = false;
        nextPhase = false;
    }

}
