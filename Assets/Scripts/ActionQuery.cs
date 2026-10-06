using System.Collections.Generic;

public class ActionQuery {
    GameRule _rule;
    public ActionQuery(GameRule rule) {
        _rule = rule;
    }

    List<ActionType> GetAction(CardBase card) {
        return card.CardType switch {
            CardType.Monster => _defaultMonsterAction,
            CardType.Spell => _defaultSpellAction,
            CardType.Trap => _defaultTrapAction,
            _ => new(),
        };
    }

    public List<ActionType> GetAvailableAction(CardBase card, ActionContext context) {
        var avalSet = GetAction(card);
        List<ActionType> list = new();
        foreach (var action in avalSet) {
            bool rulePass = _rule.CheckAction(action, card, context);
            bool effectPass = true;
            if (card.HasEffect) {
                effectPass = card.Effect.CheckCanActive(action, context);
            }
            if (effectPass && rulePass) {
                list.Add(action);
            }
        }
        return list;
    }

    List<ActionType> _defaultMonsterAction = new() {
        ActionType.NormalSummon,
        // ActionType.SpecialSummon,
        ActionType.Activate,
        ActionType.ChangePosition,
        ActionType.MonsterSet,
        ActionType.Attack,
    };
    List<ActionType> _defaultSpellAction = new() {
        ActionType.Activate,
        ActionType.SpellTrapSet,
    };
    List<ActionType> _defaultTrapAction = new() {
        ActionType.Activate,
        ActionType.SpellTrapSet,
    };
}