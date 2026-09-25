using System.Collections.Generic;

public interface IEffect {
    bool CheckCanActive(ActionType action, ActionContext context);
    void Resolve(EffectContext context);
}