public static class EffectFactory {
    public static IEffect CreateInstance(CardData data) {
        switch (data.effectType) {
            case EffectType.Healing:
                return new Effect_Healing();
            case EffectType.Damage:
                return new Effect_Damage();
            default:
                return new Effect_None();
        }
    }
}