using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Card Effects/Dice Formula")]
public class DiceFormulaEffect : CardEffect
{
    [Header("Dice")]
    public int diceToRoll = 1;

    [Header("Matching Faces")]
    public string[] requiredFace;

    public enum OutputType
    {
        Damage,
        Heal,
        CP
    }

    [Header("Result")]
    public OutputType outputType;

    [Tooltip("Use x as the number of matching faces. Example: 1+2*x")]
    public string formula = "x";

    public override IEnumerator Execute(CardContext context)
    {
        bool finished = false;
        List<string> results = null;

        context.diceManager.RollDiceFromCard(
            diceToRoll,
            rolledResults =>
            {
                results = rolledResults;
                finished = true;
            });

        while (!finished)
            yield return null;

        int count = 0;

        foreach (string result in results)
        {
            foreach (string face in requiredFace)
            {
                if (result == face)
                {
                    count++;
                    break;
                }
            }
        }

        string expression =
            formula.Replace("x", count.ToString());

        int value =
            MathUtil.Evaluate(expression);

        switch (outputType)
        {
            case OutputType.Damage:

                context.target.TakeDamage(
                    new DamagePacket
                    {
                        amount = value,
                        source = context.owner,
                        target = context.target,
                        damageType = DamageType.Normal,
                        offensiveRollDamage = false
                    });

                break;

            case OutputType.Heal:

                context.owner.Heal(value);

                break;

            case OutputType.CP:

                context.owner.GainCP(value);

                break;
        }
    }
}