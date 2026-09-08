using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

public class InputManager : MonoBehaviour {
    [SerializeField] InputActionReference _drawAction;
    [SerializeField] InputActionReference _nextAction;
    [SerializeField] InputActionReference _left;
    [SerializeField] InputActionReference _right;
    [SerializeField] InputActionReference _cancelAction;
    [SerializeField] InputActionReference _normalSummonAction;
    [SerializeField] InputActionReference _attackAction;

    Dictionary<Command, InputActionReference> _acitionsDic;

    public void Start() {
        _acitionsDic = new() {
            {Command.Draw, _drawAction},
            {Command.NextPhase, _nextAction},
            {Command.NormalSummon, _normalSummonAction},
            {Command.Attack, _attackAction},
            {Command.Cancel, _cancelAction},
            {Command.Left, _left},
            {Command.Right, _right},
        };
    }


    public void GetInput(ref InputData inputData) {
        foreach (var action in _acitionsDic) {
            if (action.Value.action.WasPressedThisFrame()) {
                inputData.Clicks[action.Key] = true;
            }
        }
    }
}

public class InputData {
    public bool draw;
    public bool nextPhase;
    public bool left;
    public bool right;
    public bool normalSummon;
    public bool cancel;
    public bool Attack;
    public Dictionary<Command, bool> Clicks;

    public InputData() {
        Clicks = new() {
             {Command.Draw, false},
             {Command.NextPhase, false},
             {Command.NormalSummon, false},
             {Command.Attack, false},
             {Command.Cancel, false},
             {Command.Left, false},
             {Command.Right, false},
        };

    }

    public void Reset() {
        foreach (var command in Clicks.Keys) {
            Clicks[command] = false;
        }
    }

}
