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
            bool rulePass = _rule.CheckAction(action, card, context);
            bool effectPass = true;
            if (card.HasEffect) {
                effectPass = card.CanActivate(action, context);
            }
            if (effectPass && rulePass) {
                list.Add(action);
            }
        }
        return list;
    }
}