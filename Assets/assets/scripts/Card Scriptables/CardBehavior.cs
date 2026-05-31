using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardBehaviour : MonoBehaviour
{
    [Header("Card Info")]
    public string cardName;

    public int cost;

    public CardType cardType;
    public ActionType actionType;

    public PlayerType targetType;

    [Header("Effects")]
    public List<CardEffect> effects = new();

    [HideInInspector]
    public CharacterState owner;

    public enum CardType
    {
        Action,
        Upgrade
    }

    public enum ActionType
    {
        Instant,
        MainPhase,
        RollPhase
    }

    public IEnumerator Execute(
        CharacterState target,
        DiceManager diceManager,
        ControlManager controlManager)
    {
        if (!owner.SpendCP(cost))
            yield break;

        CardContext context = new CardContext
        {
            owner = owner,
            target = target,
            diceManager = diceManager,
            controlManager = controlManager
        };

        foreach (CardEffect effect in effects)
        {
            yield return effect.Execute(context);
        }
    }
}