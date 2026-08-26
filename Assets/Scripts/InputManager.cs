using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

public class InputManager : MonoBehaviour {
    [SerializeField] InputActionReference _drawAction;
    [SerializeField] InputActionReference _nextAction;
    [SerializeField] InputActionReference _selectHandAction;
    [SerializeField] InputActionReference _cancelAction;
    [SerializeField] InputActionReference _normalSummonAction;

    void OnEnable() {
        _drawAction.action.Enable();
        _nextAction.action.Enable();
        _selectHandAction.action.Enable();
    }
    void OnDisable() {
        _drawAction.action.Disable();
        _nextAction.action.Disable();
        _selectHandAction.action.Disable();
    }

    public void GetInput(ref InputData inputData) {
        if (_drawAction.action.WasPressedThisFrame()) {
            inputData.draw = true;
        }
        if (_nextAction.action.WasPressedThisFrame()) {
            inputData.nextPhase = true;
        }
        if (_selectHandAction.action.WasPressedThisFrame()) {
            KeyControl keyControl = _selectHandAction.action.activeControl as KeyControl;
            inputData.selectHand = keyControl.keyCode - Key.Z;
        }
        if (_normalSummonAction.action.WasPressedThisFrame()) {
            inputData.normalSummon = true;
        }
        if (_cancelAction.action.WasPressedThisFrame()) {
            inputData.cancel = true;
        }


    }
}

public class InputData {
    public bool draw;
    public bool nextPhase;
    public int selectHand;
    public bool normalSummon;
    public bool cancel;

    public void Reset() {
        draw = false;
        nextPhase = false;
        selectHand = 0;
        normalSummon = false;
        cancel = false;
    }

}
