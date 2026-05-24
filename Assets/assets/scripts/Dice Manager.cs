using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceManager : MonoBehaviour
{
    public GameObject[] Dice = new GameObject[5];
    private Type[] possibleScripts = { typeof(BarbarianDice), typeof(ShadowThiefDice) };
    private IDice[] scriptsArray;
    public GameObject DiceLauncher;
    private DiceLuancher diceLauncher;

    void Start()
    {
        diceLauncher = DiceLauncher.GetComponent<DiceLuancher>();

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
        if (Input.GetKeyDown(KeyCode.Space))
        {
            RollAndCheckDice(5, (diceResults) =>
            {
                foreach(var dieOut in diceResults)
                print(dieOut);
            });
        }
    }

    // Pass an Action callback so the calling script receives the list when done
    public void RollAndCheckDice(int numberOfDice, Action<List<string>> onResultsReady)
    {
        StartCoroutine(WaitAndGetResults(numberOfDice, onResultsReady));
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
}