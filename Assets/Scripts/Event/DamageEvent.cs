public class DamageEvent : IGameEvent {
    public DamageType type;
    public Player source;
    public Player target;
    public CardBase sourceCard;
    public int damage;
}