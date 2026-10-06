public static class EffectFactory {
    public static IEffect CreateInstance(CardBase card, CardData data) {
        IEffect effect;
        effect = data.effectType switch {
            EffectType.Healing => new Effect_Healing(),
            EffectType.Damage => new Effect_Damage(),
            _ => new Effect_None(),
        };

        effect.Init(card);
        return effect;
    }
}