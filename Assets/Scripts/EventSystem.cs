using System;
public class EventSystem {
    public event Action<DamageEvent> OnDamageEvent;
    public void Trigger(GameEvent e) {
        if (e is DamageEvent eventInfo) {
            OnDamageEvent.Invoke(eventInfo);
        }
    }
}