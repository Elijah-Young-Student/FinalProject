using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DiceManager : MonoBehaviour
{
    [Header("Dice")]
    public GameObject[] Dice = new GameObject[5];

    public GameObject[] DiceLights = new GameObject[5];

    private List<GameObject> diceToRoll = new();

    private Type[] possibleScripts =
    {
        typeof(BarbarianDice)
    };

    private IDice[] scriptsArray;

    [Header("Launcher")]
    public GameObject DiceLauncher;

    private DiceLauncher diceLauncher;

    [Header("UI")]
    public GraphicRaycaster uiRaycaster;

    [Header("Managers")]
    public ControlManager controlManager;

    public List<string> lastResults = new();

    [Header("Modifiers")]
    private Queue<string> forcedFaces = new();

    private int bonusRerolls = 0;

    private DiceModificationRequest activeModificationRequest;

    public enum RollMode
    {
        None,
        RollAll,
        CardRoll,
        SelectionRoll
    }

    public RollMode currentRollMode;

    private bool isRolling = false;

    public bool IsRolling => isRolling;

    void Start()
    {
        diceLauncher = DiceLauncher.GetComponent<DiceLauncher>();

        List<IDice> foundScripts = new();

        foreach (GameObject die in Dice)
        {
            if (die == null)
                continue;

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

    public void StartFullRoll()
    {
        currentRollMode = RollMode.RollAll;

        GameObject[] allDice = Dice;

        RollAndCheckDice(allDice, (results) =>
        {
            Debug.Log("Full roll complete");
        });
    }

    public void RollDiceFromCard(int amount, Action<List<string>> callback)
    {
        currentRollMode = RollMode.CardRoll;

        RollAndCheckDice(amount, (results) =>
        {
            currentRollMode = RollMode.None;
            callback?.Invoke(results);
        });
    }

    public void StartSelectionRoll()
    {
        currentRollMode = RollMode.SelectionRoll;
        diceToRoll.Clear();
    }

    void Update()
    {
        // if (controlManager.currentPhase != ControlManager.GamePhase.RollOffence)
        //     return;
        if (Input.GetKeyDown(KeyCode.Space))
        {
            RollAndCheckDice(5, (diceResults) =>
            {
                foreach (var dieOut in diceResults)
                    print(dieOut);
            });
        }

        if (currentRollMode != RollMode.SelectionRoll)
            return;

        if (Input.GetMouseButtonDown(0))
        {
            if (isRolling)
                return;

            PointerEventData pointerData = new PointerEventData(EventSystem.current);
            pointerData.position = Input.mousePosition;

            List<RaycastResult> uiResults = new();
            uiRaycaster.Raycast(pointerData, uiResults);

            foreach (RaycastResult result in uiResults)
            {
                TextMeshProUGUI tmpText = result.gameObject.GetComponentInParent<TextMeshProUGUI>();

                if (tmpText)
                {
                    if (diceToRoll.Count > 0)
                    {
                        RollAndCheckDice(diceToRoll.ToArray(), (diceResults) =>
                        {
                            foreach (var dieOut in diceResults)
                                print(dieOut);
                        });

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

                if (diceToRoll.Contains(item))
                {
                    diceToRoll.Remove(item);
                }
                else
                {
                    if (Dice.Contains(item))
                    {
                        diceToRoll.Add(item);
                    }
                }
            }

            LightChosenDie(diceToRoll);
        }
    }

    public void StartDiceModification(int numberOfDice, bool anyFace, string[] allowedFaces = null)
    {
        activeModificationRequest = new DiceModificationRequest
        {
            awaitingSelection = true,
            diceToModify = numberOfDice,
            allowAnyFace = anyFace,
            allowedFaces = allowedFaces
        };
    }

    public void EnableBonusRerolls(int amount)
    {
        bonusRerolls += amount;
    }

    public void ForceNextDice(string face)
    {
        forcedFaces.Enqueue(face);
    }

    public void StartRollPhase()
    {

    }

    private void LightChosenDie(List<GameObject> diceToRoll)
    {
        for (int i = 0; i < Dice.Length; i++)
        {
            if (Dice[i] == null || DiceLights[i] == null)
                continue;

            DiceLights[i].SetActive(diceToRoll.Contains(Dice[i]));
        }
    }

    public void RollAndCheckDice(int numberOfDice, Action<List<string>> onResultsReady)
    {
        if (isRolling)
            return;
        isRolling = true;
        StartCoroutine(WaitAndGetResults(numberOfDice, onResultsReady));

    }

    public void RollAndCheckDice(GameObject[] dice, Action<List<string>> onResultsReady)
    {
        if (isRolling)
            return;
        isRolling = true;
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
                    if (!rb.IsSleeping() && rb.velocity.sqrMagnitude > 0.001f)
                    {
                        allDiceStopped = false;
                        break;
                    }
                }
            }

            yield return null;
        }

        List<string> finalResults = GetDiceResults(numberOfDice);

        lastResults = finalResults;

        yield return new WaitForSeconds(2.5f);

        foreach (GameObject die in Dice)
        {
            diceLauncher.ReturnDiceToOrign(die);
        }

        isRolling = false;
        callback?.Invoke(finalResults);
    }

    private IEnumerator WaitAndGetResults(GameObject[] dice, Action<List<string>> callback)
    {
        List<int> indexes = new();

        foreach (GameObject die in dice)
        {
            foreach (GameObject d in Dice)
            {
                if (die == d)
                {
                    int i = Array.IndexOf(Dice, d);

                    diceLauncher.Launch(Dice[i]);

                    indexes.Add(i);
                }
            }
        }

        yield return new WaitForSeconds(1f);

        bool allDiceStopped = false;

        while (!allDiceStopped)
        {
            allDiceStopped = true;

            for (int i = 0; i < indexes.Count; i++)
            {
                Rigidbody rb = scriptsArray[indexes[i]].GetRigidbody();

                if (rb != null)
                {
                    if (!rb.IsSleeping() && rb.velocity.sqrMagnitude > 0.001f)
                    {
                        allDiceStopped = false;
                        break;
                    }
                }
            }

            yield return null;
        }

        List<string> finalResults = GetDiceResults(indexes.ToArray());

        lastResults = finalResults;

        yield return new WaitForSeconds(2.5f);

        foreach (GameObject die in dice)
        {
            diceLauncher.ReturnDiceToOrign(die);
        }

        isRolling = false;
        callback?.Invoke(finalResults);
    }

    private List<string> GetDiceResults(int numberOfDice)
    {
        List<string> result = new();

        if (numberOfDice > 0)
        {
            int loops = Mathf.Min(numberOfDice, scriptsArray.Length);

            for (int i = 0; i < loops; i++)
            {
                string face;

                if (forcedFaces.Count > 0)
                {
                    face = forcedFaces.Dequeue();
                }
                else
                {
                    face = scriptsArray[i].GetFaceSide();
                }

                result.Add(face);
            }
        }

        return result;
    }

    private List<string> GetDiceResults(int[] dieScriptIndex)
    {
        List<string> result = new();

        foreach (int i in dieScriptIndex)
        {
            string face;

            if (forcedFaces.Count > 0)
            {
                face = forcedFaces.Dequeue();
            }
            else
            {
                face = scriptsArray[i].GetFaceSide();
            }

            result.Add(face);
        }

        return result;
    }

    public void RerollSelectedDice(List<GameObject> selectedDice, int maxRerolls, Action<List<string>> callback)
    {
        if (isRolling)
            return;

        StartCoroutine(RerollRoutine(selectedDice, maxRerolls, callback));
    }

    private IEnumerator RerollRoutine(List<GameObject> selectedDice, int maxRerolls, Action<List<string>> callback)
    {
        isRolling = true;

        // 1. CLAMP LIST SIZE (REMOVE FROM END)
        if (selectedDice.Count > maxRerolls)
        {
            int removeCount = selectedDice.Count - maxRerolls;

            for (int i = 0; i < removeCount; i++)
            {
                selectedDice.RemoveAt(selectedDice.Count - 1);
            }
        }

        List<int> indexes = new();

        // 2. VALIDATE + LAUNCH
        foreach (GameObject die in selectedDice)
        {
            int index = Array.IndexOf(Dice, die);

            if (index != -1)
            {
                indexes.Add(index);
                diceLauncher.Launch(Dice[index]);
            }
        }

        yield return new WaitForSeconds(1f);

        // 3. WAIT FOR STOP
        bool allStopped = false;

        while (!allStopped)
        {
            allStopped = true;

            foreach (int i in indexes)
            {
                Rigidbody rb = scriptsArray[i].GetRigidbody();

                if (rb != null &&
                    !rb.IsSleeping() &&
                    rb.velocity.sqrMagnitude > 0.001f)
                {
                    allStopped = false;
                    break;
                }
            }

            yield return null;
        }

        // 4. GET RESULTS
        List<string> results = GetDiceResults(indexes.ToArray());
        lastResults = results;

        yield return new WaitForSeconds(0.5f);

        isRolling = false;

        callback?.Invoke(results);
    }
}