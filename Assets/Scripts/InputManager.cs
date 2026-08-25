using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour {
    [SerializeField] InputActionReference _drawAction;
    [SerializeField] InputActionReference _jumpAction;
    [SerializeField] InputActionReference _mouseAction;
    [SerializeField] InputActionReference _rightClickAction;
    [SerializeField] InputActionReference _leftClickAction;

    void OnEnable() {
        _drawAction.action.Enable();
        _jumpAction.action.Enable();
        _mouseAction.action.Enable();
        _rightClickAction.action.Enable();
        _leftClickAction.action.Enable();
    }
    void OnDisable() {
        _drawAction.action.Disable();
        _jumpAction.action.Disable();
        _mouseAction.action.Disable();
        _rightClickAction.action.Disable();
        _leftClickAction.action.Disable();
    }

    public void GetInput(ref InputData inputData) {
        if (_drawAction.action.WasPressedThisFrame()) {
            inputData.draw = true;
        }
        Vector2 screenPos = _mouseAction.action.ReadValue<Vector2>();
        inputData.mousePos = Camera.main.ScreenToWorldPoint(screenPos);
        if (_jumpAction.action.WasPressedThisFrame()) {
            inputData.jump = true;
        }
        if (_leftClickAction.action.WasPressedThisFrame()) {
            inputData.attack = true;
        }

        inputData.onFocus = _rightClickAction.action.IsPressed();

    }
}

public class InputData {
    public bool draw;
    public bool jump;
    public Vector2 mousePos;
    public bool onFocus;
    public bool attack;

}
