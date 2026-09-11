using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

public class InputManager : MonoBehaviour {
    [SerializeField] InputActionReference _confirmAction;
    [SerializeField] InputActionReference _left;
    [SerializeField] InputActionReference _right;
    [SerializeField] InputActionReference _up;
    [SerializeField] InputActionReference _down;
    [SerializeField] InputActionReference _cancelAction;

    Dictionary<InputType, InputActionReference> _acitionsDic;

    public void Start() {
        _acitionsDic = new() {
            {InputType.Confirm, _confirmAction},
            {InputType.Cancel, _cancelAction},
            {InputType.Left, _left},
            {InputType.Right, _right},
        };
    }


    public void GetInput(ref InputData inputData) {
        foreach (var action in _acitionsDic) {
            if (action.Value.action.WasPressedThisFrame()) {
                inputData[action.Key] = true;
            }
        }
    }
}

public class InputData {
    Dictionary<InputType, bool> _clicks;
    public bool this[InputType key] {
        get {
            return _clicks[key];
        }
        set {
            _clicks[key] = value;
        }
    }


    public InputData() {
        _clicks = new() {
            {InputType.Confirm, false},
            {InputType.Cancel, false},
            {InputType.Left, false},
            {InputType.Right, false},
        };

    }

    public void Reset() {
        foreach (var command in _clicks.Keys) {
            _clicks[command] = false;
        }
    }

}
