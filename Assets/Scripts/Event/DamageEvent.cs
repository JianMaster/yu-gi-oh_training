public class DamageEvent : GameEvent {
    public DamageType type;
    public Player source;
    public Player target;
    public CardBase sourceCard;
    public int damage;
}