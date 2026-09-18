using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

public class InputManager : MonoBehaviour {
    [SerializeField] InputActionReference _confirm;
    [SerializeField] InputActionReference _left;
    [SerializeField] InputActionReference _right;
    [SerializeField] InputActionReference _up;
    [SerializeField] InputActionReference _down;
    [SerializeField] InputActionReference _cancel;
    [SerializeField] InputActionReference _nextPhase;

    Dictionary<InputType, InputActionReference> _acitionsDic;

    public void Start() {
        _acitionsDic = new() {
            {InputType.Confirm, _confirm},
            {InputType.Cancel, _cancel},
            {InputType.Left, _left},
            {InputType.Right, _right},
            {InputType.Up, _up},
            {InputType.Down, _down},
            {InputType.NextPhase, _nextPhase},
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
            {InputType.Up, false},
            {InputType.Down, false},
            {InputType.NextPhase, false},
        };

    }

    public void Reset() {
        foreach (var command in _clicks.Keys.ToList()) {
            _clicks[command] = false;
        }
    }

}
