using System.Collections.Generic;

public class ActionQuery {
    GameRule _rule;
    Dictionary<ActionType, IGameAction> _actionMap;
    public ActionQuery(GameRule rule) {
        _rule = rule;
        _actionMap = new() {
            { ActionType.NormalSummon, new Action_NormalSummon() },
            // { ActionType.Attack, new Action_Attack() },
            // { ActionType.Activate, new Action_Activate() },
        };
    }

    List<ActionType> GetAction(CardBase card) {
        return card.CardType switch {
            CardType.Monster => _defaultMonsterAction,
            CardType.Spell => _defaultSpellAction,
            CardType.Trap => _defaultTrapAction,
            _ => new(),
        };
    }

    public List<IGameAction> GetAvailableAction(CardBase card, Player player, GameState state) {
        var avalSet = GetAction(card);
        List<IGameAction> list = new();
        foreach (var type in avalSet) {
            IGameAction action = _actionMap[type];
            bool rulePass = action.CanUse(card, player, state);
            bool effectPass = true;
            if (card.HasEffect) {
                // effectPass = card.Effect.CheckCanActive(actionType, context);
            }
            if (effectPass && rulePass) {
                list.Add(action);
            }
        }
        return list;
    }

    List<ActionType> _defaultMonsterAction = new() {
        ActionType.NormalSummon,
        ActionType.Attack,
        ActionType.Activate,
        // ActionType.SpecialSummon,
        // ActionType.ChangePosition,
        // ActionType.MonsterSet,
    };
    List<ActionType> _defaultSpellAction = new() {
        ActionType.Activate,
        // ActionType.SpellTrapSet,
    };
    List<ActionType> _defaultTrapAction = new() {
        ActionType.Activate,
        // ActionType.SpellTrapSet,
    };
}