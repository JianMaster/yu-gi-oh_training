using System.Collections.Generic;

public class ActionQuery {
    GameRule _rule;
    public ActionQuery(GameRule rule) {
        _rule = rule;
    }
    public List<ActionType> GetAvailableAction(CardBase card, ActionContext context) {
        var avalSet = card.GetAction();
        List<ActionType> list = new();
        foreach (var action in avalSet) {
            bool available = _rule.CheckAction(action, card, context);
            if (action == ActionType.Activate && card.HasEffect) {
                available = available && card.CanActivate();
            }
            if (available) {
                list.Add(action);
            }
        }
        return avalSet;
    }
}