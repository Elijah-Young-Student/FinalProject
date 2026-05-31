using System.Collections;
using UnityEngine;

[CreateAssetMenu(menuName = "Card Effects/Draw Card")]
public class DrawCardEffect : CardEffect
{
    [Header("Amount to draw")]
    public int amount = 1;

    public override IEnumerator Execute(CardContext context)
    {
        if (context == null || context.owner == null)
            yield break;

        CardManager cardManager = context.owner.cardManager;

        if (cardManager == null)
        {
            Debug.LogWarning("DrawCardEffect: No CardManager found on owner.");
            yield break;
        }

        for (int i = 0; i < amount; i++)
        {
            cardManager.DrawCard();
        }

        yield return null;
    }
}