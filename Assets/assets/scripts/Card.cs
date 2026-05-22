using UnityEngine;

[CreateAssetMenu(fileName = "New Card", menuName = "Cards/Card")]
public class Card : ScriptableObject
{
    public GameObject cardPrefab;

    public string cardName = "Dont touch just name the card what you want it called";

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
        RollPhase,
        Passive
    }

    public enum CardType
    {
        Upgrade,
        Action
    }
}