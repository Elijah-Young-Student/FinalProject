using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Card", menuName = "Cards/Card")]
public class Card : ScriptableObject
{
    public GameObject cardPrefab;

    public string cardName = "Dont touch just name the card what you want it called";

    public int cost;

    public bool cardDraws;

    public int cardsToDraw1;

    public int cardsToDraw2;

    public bool CPChange;

    public int CPToGain1;

    public int CPToGain2;

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

    /// <summary>
    /// 10=playPhase, 11=cardType,
    /// 0=healthImpact, 1=cardDraws,
    /// 2=targretCardDraws (code), 3=cpCost, 4=StatusImpact (code),
    /// 5=diceReRoleAmount, 6=targetDiceReRole, 7=diceChange (code),
    /// 8=targetDmgImpact
    /// </summary>
    private List<object> list = new List<object>();

    public List<object> CardOutcomes(string[] diceResult, string character = "", bool shadows = false, int dmgDelt = 0)
    {
        list[10] = actionType;
        list[11] = cardType;
        list[3] = cost;
        if (name == "Adrenaline Surge!")
        {
            if (diceResult[0][1] == '✷')
            {
                list[0] = 2;
                list[4] = "Co1";
            }
            else list[1] = 1;
        }
        else if (name == "Better D!")
        {
            list[6] = 5;
        }
        else if (name == "Buh, Bye!")
        {
            list[4] = "S-1";
        }
        else if (name == "Card Trick!")
        {
            list[2] = "D-1R";
            if (shadows)
            {
                list[1] = 2;
            }
            else list[1] = 1;
        }
        else if (name == "Caducopia II")
        {
            //upgrades need to be figured out yet
        }
        else if (name == "Counter Strike II")
        {
            //upgrades need to be figured out yet
        }
        else if (name == "Crit Bash II")
        {
            //upgrades need to be figured out yet
        }
        else if (name == "Dagger Strike II")
        {
            //upgrades need to be figured out yet
        }
        else if (name == "Double Up!")
        {
            list[1] = 2;
        }
        else if (name == "Enter The Shadows!")
        {
            list[4] = "Sh=t";
        }
        else if (name == "Feelin' Good!")
        {
            int h = 0;
            foreach (string die in diceResult)
            {
                if (die[1] == '❤') h++;
            }
            list[0] = 1 + 2 * h;
        }
        else if (name == "Fortitude II")
        {
            //upgrades need to be figured out yet
        }
        else if (name == "Get Some!")
        {
            int s = 0;
            foreach (string die in diceResult)
            {
                if (die[1] == 's') s++;
            }
            list[8] = s;
            list[4] = "Co1";
        }
        else if (name == "Get That Outta Here!")
        {
            list[4] = "S-1";
        }
        else if (name == "Getting Paid!")
        {
            list[9] = 2;
        }
        else if (name == "Head Bash!")
        {
            if (dmgDelt >= 8)
                list[4] = "Co1";
        }
        else if (name == "Helping hand!")
        {
            list[6] = 1;
        }
        else if (name == "Insidious Strike II x Shank Attack")
        {
            //upgrades need to be figured out yet
        }
        else if (name == "Mighty Blow II")
        {
            //upgrades need to be figured out yet
        }
        else if (name == "Not This Time!")
        {
            list[8] = -6;
        }
        else if (name == "One More Time!")
        {
            list[5] = 5;
        }
        else if (name == "Overpower II")
        {
            //upgrades need to be figured out yet
        }
        else if (name == "Pickpcket II")
        {
            //upgrades need to be figured out yet
        }
        else if (name == "Poison Wound!")
        {
            list[4] = "Po1";
        }
        else if (name == "Reckless II")
        {
            //upgrades need to be figured out yet
        }
        else if (name == "Samsiesk!")
        {
            list[6] = "1Dm";
        }
        else if (name == "Shadow Coin!")
        {
            if (shadows) list[3] = 3;
            else list[3] = 2;
        }
        else if (name == "Shadow Dance!")
        {
            //upgrades need to be figured out yet
        }
        else if (name == "Shadow Defense II")
        {
            //upgrades need to be figured out yet
        }
        else if (name == "Shadow Meld!")
        {
            if (diceResult[0][1] == 'p')
            {
                list[4] = "SA1";
                list[3] = 2;
            }
            else list[1] = 1;
        }
        else if (name == "Shifty Strike II x Shadow Strike")
        {
            //upgrades need to be figured out yet
        }
        else if (name == "Six It!")
        {
            list[2] = "Pcd-6";
        }
        else if (name == "Smack II")
        {
            //upgrades need to be figured out yet
        }
        else if (name == "Smack III")
        {
            //upgrades need to be figured out yet
        }
        else if (name == "So Wild!")
        {
            list[2] = "TdW";
        }
        else if (name == "Sturdy Blow II")
        {
            //upgrades need to be figured out yet
        }
        else if (name == "Sturdy Blow III")
        {
            //upgrades need to be figured out yet
        }
        else if (name == "Thick Skin II")
        {
            //upgrades need to be figured out yet
        }
        else if (name == "Tip It!")
        {
            list[2] = "Td+-1";
        }
        else if (name == "Transference!")
        {
            list[4] = "T1S";
        }
        else if (name == "Triple Up!")
        {
            list[1] = 3;
        }
        else if (name == "Try, Try Again!")
        {
            list[2] = "Tr2";
        }
        else if (name == "Twice As Wild!")
        {
            list[2] = "Td2W";
        }
        else if (name == "Vegas Baby!")
        {
            list[9] = Mathf.RoundToInt(diceResult[0][0] / 2);
        }
        else if (name == "What Status Effects")
        {
            list[4] = "RAFT";
        }
        else if (name == "Wild Shadow!")
        {
            if (shadows) list[2] = "Td2W";
            else list[2] = "TdW";
        }
        return list;
    }
}