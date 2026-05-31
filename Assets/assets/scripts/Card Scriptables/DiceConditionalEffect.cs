using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Card Effects/Dice Conditional")]
public class DiceConditionalEffect : CardEffect
{
    [Header("Condition")]
    public string key = "diceResults";
    public string requiredFace = "6✷";

    [Header("Branch Effects")]
    public List<CardEffect> trueEffects = new();
    public List<CardEffect> falseEffects = new();

    public override IEnumerator Execute(CardContext context)
    {
        // Get stored dice results
        if (!context.data.TryGetValue(key, out object raw) || raw == null)
        {
            Debug.LogWarning("DiceConditionalEffect: No dice results found in context.");
            yield break;
        }

        List<string> results = raw as List<string>;

        if (results == null)
        {
            Debug.LogWarning("DiceConditionalEffect: Invalid dice result format.");
            yield break;
        }

        // CONDITION CHECK
        bool success = results.Contains(requiredFace);

        // PICK BRANCH
        List<CardEffect> branch = success ? trueEffects : falseEffects;

        // EXECUTE BRANCH EFFECTS
        for (int i = 0; i < branch.Count; i++)
        {
            if (branch[i] == null)
                continue;

            yield return branch[i].Execute(context);
        }
    }
}