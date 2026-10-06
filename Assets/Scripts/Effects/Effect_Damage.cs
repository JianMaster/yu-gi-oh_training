using System.Collections.Generic;

public class Effect_Damage : IEffect {
    CardBase _source;
    public CardBase Source => _source;

    public void Init(CardBase source) {
        _source = source;
    }
    public bool CheckCanActive(ActionType action, ActionContext context) {
        return true;
    }
    public void Resolve(EffectContext context) {
        context.opponent.TakeDamage(context.value);
    }
}