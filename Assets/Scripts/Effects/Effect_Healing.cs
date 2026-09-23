using UnityEngine;

public class Effect_Healing : IEffect {
    public void Resolve(EffectContext context) {
        context.target.Heal(context.value);
    }
}