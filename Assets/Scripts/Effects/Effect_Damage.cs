using System.Collections.Generic;

public class Effect_Damage : IEffect {
    public bool CheckCanActive(ActionType action, ActionContext context) {
        return true;
    }
    public void Resolve(EffectContext context) {
        context.target.TakeDamage(context.value);
    }
}