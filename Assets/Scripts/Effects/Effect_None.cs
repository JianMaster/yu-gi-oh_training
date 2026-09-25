using System.Collections.Generic;

public class Effect_None : IEffect {
    public bool CheckCanActive(ActionType action, ActionContext context) { return true; }
    public void Resolve(EffectContext context) { }
}