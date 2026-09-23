using System;
public class EventSystem {
    public event Action<DamageEvent> OnDamageEvent;
    public void Trigger(IGameEvent e) {
        if (e is DamageEvent eventInfo) {
            OnDamageEvent.Invoke(eventInfo);
        }
    }
}