using System.Collections.Generic;
using UnityEngine;

public class Effect_Healing : IEffect {
    CardBase _source;
    public CardBase Source => _source;

    public void Init(CardBase source) {
        _source = source;
    }

    public bool CheckCanActive(ActionType action, ActionContext context) {
        return true;
    }

    public void Resolve(EffectContext context) {
        context.activater.Heal(context.value);
    }
}