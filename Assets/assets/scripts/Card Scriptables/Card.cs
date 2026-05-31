using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Cards/Card")]
public class Card : ScriptableObject
{
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
    
    [Header("Identity")]
    public string cardName;

    [Header("Core")]
    public int cost;

    public CardType cardType;
    public ActionType actionType;

    [Header("Targeting")]
    public PlayerType targetType;

    [Header("Effects")]
    public List<CardEffect> effects;
}