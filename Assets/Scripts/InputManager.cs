using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour {
    [SerializeField] InputActionReference _drawAction;

    void OnEnable() {
        _drawAction.action.Enable();
    }
    void OnDisable() {
        _drawAction.action.Disable();
    }

    public void GetInput(ref InputData inputData) {
        if (_drawAction.action.WasPressedThisFrame()) {
            inputData.draw = true;
        }

    }
}

public class InputData {
    public bool draw;

    public void Reset() {
        draw = false;
    }

}
