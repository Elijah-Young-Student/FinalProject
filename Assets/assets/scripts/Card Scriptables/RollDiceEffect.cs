using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Card Effects/Roll Dice")]
public class RollDiceEffect : CardEffect
{
    public int diceCount;

    public override IEnumerator Execute(CardContext context)
    {
        bool done = false;
        List<string> results = null;

        context.diceManager.RollAndCheckDice(
            diceCount,
            r =>
            {
                results = r;
                done = true;
            });

        yield return new WaitUntil(() => done);

        context.diceManager.lastResults = results;
    }
}