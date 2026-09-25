using System.Collections.Generic;
using UnityEngine;

public class Effect_Healing : IEffect {
    public bool CheckCanActive(ActionType action, ActionContext context) {
        return true;
    }

    public void Resolve(EffectContext context) {
        context.target.Heal(context.value);
    }
}