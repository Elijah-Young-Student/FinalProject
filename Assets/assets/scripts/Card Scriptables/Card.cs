using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Card", menuName = "Cards/Card")]
public class Card : ScriptableObject
{
    [Header("Basic Info")]
    public string cardName;

    public int cost;

    [Header("Card Type")]
    public ActionType actionType;

    public CardType cardType;

    [Header("Effects")]
    public List<CardEffect> effects;

    public enum ActionType
    {
        Instant,
        MainPhase,
        RollPhase,
        Passive
    }

    public enum CardType
    {
        Upgrade,
        Action
    }
}