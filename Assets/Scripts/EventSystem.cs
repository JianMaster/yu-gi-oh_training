using System;
public static class EventSystem {
    public static event Action ChangeOperator;
    public static event Action NextTurn;
    public static event Action<DamageEvent> OnDamageEvent;
    public static void Trigger(IGameEvent e) {
        if (e is DamageEvent eventInfo) {
            OnDamageEvent?.Invoke(eventInfo);
        }
    }
}