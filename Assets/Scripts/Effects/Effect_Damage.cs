public class Effect_Damage : IEffect {
    public void Resolve(EffectContext context) {
        context.target.TakeDamage(context.value);
    }
}