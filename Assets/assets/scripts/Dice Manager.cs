using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static ControlManager;

public class DiceManager : MonoBehaviour
{
    public GameObject[] Dice = new GameObject[5];
    private List<GameObject> diceToRoll = new List<GameObject>(1);
    private Type[] possibleScripts = { typeof(BarbarianDice), typeof(ShadowThiefDice) };
    private IDice[] scriptsArray;
    public GameObject DiceLauncher;
    private DiceLauncher diceLauncher;

    public GraphicRaycaster uiRaycaster;

    public GameObject[] DiceLights = new GameObject[5];


    void Start()
    {
        diceLauncher = DiceLauncher.GetComponent<DiceLauncher>();

        List<IDice> foundScripts = new List<IDice>();
        foreach (GameObject die in Dice)
        {
            if (die == null) continue;
            foreach (Type scriptType in possibleScripts)
            {
                IDice script = die.GetComponent(scriptType) as IDice;
                if (script != null)
                {
                    foundScripts.Add(script);
                    break;
                }
            }
        }
        scriptsArray = foundScripts.ToArray();
    }

    void Update()
    {

            if (FindObjectOfType<ControlManager>().currentPhase != GamePhase.RollOffence)
                return;

            // dice selection / reroll logic here
  
        if (Input.GetKeyDown(KeyCode.Space))
        {
            RollAndCheckDice(5, (diceResults) =>
            {
                foreach (var dieOut in diceResults)
                    print(dieOut);
            });
        }

        if (Input.GetMouseButtonDown(0))
        {
            // detect UI pressed
            PointerEventData pointerData = new PointerEventData(EventSystem.current);
            pointerData.position = Input.mousePosition;

            List<RaycastResult> uiResults = new List<RaycastResult>();
            uiRaycaster.Raycast(pointerData, uiResults);

            foreach (RaycastResult result in uiResults)
            {
                TextMeshProUGUI tmpText = result.gameObject.GetComponentInParent<TextMeshProUGUI>();
                if (tmpText)
                {
                    // cant reroll nothing
                    if (diceToRoll.Count > 0)
                    {
                        print("reroll pressed");
                        // Thread.Sleep(500);
                        RollAndCheckDice(diceToRoll.ToArray(), (diceResults) =>
                        {
                            foreach (var dieOut in diceResults)
                                print(dieOut);
                        });
                        // clear the list so the same dice cant be rerolled imidialty on accident
                        diceToRoll.Clear();
                    }
                    return;
                }
            }
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                GameObject item = hit.collider.gameObject;
                print(item.name);
                // see if dice is already to be rolled, and if it is it is removed from the roll 'que'
                if (diceToRoll.Contains(item))
                {
                    diceToRoll.Remove(item);
                }
                else
                {
                    // if dice is not in the 'que' adds it if it is one of the dice to roll
                    if (Dice.Contains(item))
                    {
                        diceToRoll.Add(item);
                    }
                }
            }
            LightChosenDie(diceToRoll);
        }
        
    }

    public void StartRollPhase()
    {
        //RollAllDice((dice) =>
        //{
        //    // optional: notify ControlManager later
        //});
    }

    // turn on and off the lights so the player knows what dice they have selected to reroll
    private void LightChosenDie(List<GameObject> diceToRoll)
    {
        for (int i = 0; i < Dice.Length; i++)
        {
            if (Dice[i] == null || DiceLights[i] == null) continue;
            DiceLights[i].SetActive(diceToRoll.Contains(Dice[i]));
        }
    }

    // Pass an Action callback so the calling script receives the list when done
    public void RollAndCheckDice(int numberOfDice, Action<List<string>> onResultsReady)
    {
        StartCoroutine(WaitAndGetResults(numberOfDice, onResultsReady));
    }

    public void RollAndCheckDice(GameObject[] dice, Action<List<string>> onResultsReady)
    {
        StartCoroutine(WaitAndGetResults(dice, onResultsReady));
    }

    private IEnumerator WaitAndGetResults(int numberOfDice, Action<List<string>> callback)
    {

        int loops = Mathf.Min(numberOfDice, scriptsArray.Length);
        for (int i = 0; i < loops; i++)
        {
            diceLauncher.Launch(Dice[i]);
        }

        yield return new WaitForSeconds(1f);

        bool allDiceStopped = false;

        while (!allDiceStopped)
        {
            allDiceStopped = true;

            for (int i = 0; i < loops; i++)
            {
                Rigidbody rb = scriptsArray[i].GetRigidbody();
                if (rb != null)
                {
                    // Using .velocity for backwards compatibility
                    if (!rb.IsSleeping() && rb.velocity.sqrMagnitude > 0.001f)
                    {
                        allDiceStopped = false;
                        break;
                    }
                }
            }
            yield return null;
        }

        // Gather the results now that everything is resting
        List<string> finalResults = GetDiceResults(numberOfDice);

        //set the dice back to there origin after delay
        yield return new WaitForSeconds(2.5f);
        foreach (GameObject die in Dice)
        {
            diceLauncher.ReturnDiceToOrign(die);
        }

        // Trigger the callback and hand over the list to whoever called this method
        callback?.Invoke(finalResults);
    }

    private IEnumerator WaitAndGetResults(GameObject[] dice, Action<List<string>> callback)
    {
        List<int> j = new List<int>();
        foreach (GameObject die in dice)
        {
            foreach (GameObject d in Dice)
            {
                if (die == d)
                {
                    // get the index of the item in the original array
                    int i = Array.IndexOf(Dice, d);
                    diceLauncher.Launch(Dice[i]);
                    j.Add(i);
                }
            }
        }


        yield return new WaitForSeconds(1f);

        bool allDiceStopped = false;

        while (!allDiceStopped)
        {
            allDiceStopped = true;
            for (int i = 0; i < j.Count; i++)
            {
                Rigidbody rb = scriptsArray[j[i]].GetRigidbody();
                if (rb != null)
                {
                    // Using .velocity for backwards compatibility
                    if (!rb.IsSleeping() && rb.velocity.sqrMagnitude > 0.001f)
                    {
                        allDiceStopped = false;
                        break;
                    }
                }
            }
            yield return null;
        }

        // Gather the results now that everything is resting
        List<string> finalResults = GetDiceResults(j.ToArray());

        //set the dice back to there origin after delay
        yield return new WaitForSeconds(2.5f);
        foreach (GameObject die in dice)
        {
            diceLauncher.ReturnDiceToOrign(die);
        }

        // Trigger the callback and hand over the list to whoever called this method
        callback?.Invoke(finalResults);
    }

    private List<string> GetDiceResults(int numberOfDice)
    {
        List<string> result = new List<string>();
        if (numberOfDice > 0)
        {
            int loops = Mathf.Min(numberOfDice, scriptsArray.Length);
            for (int i = 0; i < loops; i++)
            {
                result.Add(scriptsArray[i].GetFaceSide());
            }
        }
        return result;
    }

    private List<string> GetDiceResults(int[] dieScriptIndex)
    {
        List<string> result = new List<string>();
        foreach (int i in dieScriptIndex)
            result.Add(scriptsArray[i].GetFaceSide());
        return result;
    }
}