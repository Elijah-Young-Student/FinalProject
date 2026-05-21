using UnityEngine;

[CreateAssetMenu(fileName = "New Card", menuName = "Cards/Card")]
public class Card : ScriptableObject
{
    public GameObject cardPrefab;

    public string cardName;

    [TextArea(3, 5)]
    public string description;

    public int cost;

    public bool cardDraws;

    public int cardsToDraw;

    public bool CPChange;

    public int CPToGain;

    public ActionType actionType;

    public CardType cardType;

    public enum ActionType
    {
        Instant,
        MainPhase,
        DefensivePhase,
        RollPhase,
        Passive
    }

    public enum CardType
    {
        Upgrade,
        Action
    }
}