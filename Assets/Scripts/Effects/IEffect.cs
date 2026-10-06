using System.Collections.Generic;

public interface IEffect {
    CardBase Source { get; }
    void Init(CardBase sourceCard);
    bool CheckCanActive(ActionType action, ActionContext context);
    void Resolve(EffectContext context);
}