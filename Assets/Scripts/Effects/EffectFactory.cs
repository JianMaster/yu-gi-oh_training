public static class EffectFactory {
    public static IEffect CreateInstance(CardBase card) {
        IEffect effect;
        effect = card.Data.effectType switch {
            EffectType.Healing => new Effect_Healing(),
            EffectType.Damage => new Effect_Damage(),
            _ => new Effect_None(),
        };

        effect.Init(card);
        return effect;
    }
}