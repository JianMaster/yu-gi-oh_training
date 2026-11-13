using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController {
    GameState _gameState;
    DuelAction _duelAction;
    public GameController(GameState state) {
        _gameState = state;
    }

    public void GameStart() {
        Debug.Log("游戏开始！");
        _duelAction.Draw(_gameState.TurnOwner, GameDefines.START_CARD_COUNT);
        _duelAction.Draw(_gameState.Opponent, GameDefines.START_CARD_COUNT);
    }

    public void ExcuteCommand(IPlayerAction action, ActionContext ctx) {
        if (ctx.nextPhase) {
            NextPhase();
            return;
        }
        if (action == null) {
            return;
        }

        action.Execute(ctx, _duelAction);
    }

    // void Activate(Action_Activate activate) {
    //     var player = activate.Executer;
    //     var card = activate.SelectCard;
    //     var toZoneId = activate.toZoneId;
    //     var fromZone = card.ZoneType;
    //     var toZone = activate.toZone;
    //     var effect = card.Effect;
    //     if (card.Owner != player) {
    //         Debug.LogError("卡牌不属于该玩家");
    //         return;
    //     }

    //     EffectContext context = new() {
    //         activater = player,
    //         opponent = _gameState.GetOpponent(player),
    //     };
    //     if (fromZone != toZone) {
    //         player.MoveCard(card, ZoneType.SpellTrap, toZoneId);
    //     }

    //     if (fromZone == toZone && card.CardType != CardType.Monster) {
    //         card.Flip();
    //     }

    //     effect.Resolve(context);

    //     if (card.CardType != CardType.Monster) {
    //         player.MoveCard(card, ZoneType.GY);
    //     }
    // }

    void NextPhase() {
        _gameState.NextPhase();
        if (_gameState.CurPhase == Phase.Draw && _gameState.Turn == 1) {
            _duelAction.Draw(_gameState.TurnOwner, GameDefines.DRAW_CARD_COUNT);
        }
    }
}