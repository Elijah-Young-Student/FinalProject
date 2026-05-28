//using System;
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//[CreateAssetMenu(fileName = "New Card", menuName = "Cards/Card")]
//public class Card : ScriptableObject
//{
//    public string cardName = "";

//    public int cost;

//    public bool cardDraws;

//    public int cardsToDraw1;

//    public int cardsToDraw2;

//    public bool CPChange;

//    public int CPToGain1;

//    public int CPToGain2;

//    public ActionType actionType;

//    public CardType cardType;

//    public enum ActionType
//    {
//        Instant,
//        MainPhase,
//        RollPhase,
//        Passive
//    }

//    public enum CardType
//    {
//        Upgrade,
//        Action
//    }
//}
using UnityEngine;

[CreateAssetMenu(fileName = "New Card", menuName = "Cards/Card")]
public class Card : ScriptableObject
{
    [Header("Basic Info")]
    public string cardName;

    public int cost;

    public Sprite artwork;

    public GameObject cardPrefab;

    [Header("Card Type")]
    public ActionType actionType;

    public CardType cardType;

    //[Header("Behaviour")]
    //public CardEffect effect;

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