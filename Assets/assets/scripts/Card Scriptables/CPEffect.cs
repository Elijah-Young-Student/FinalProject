using System.Collections;
using UnityEngine;

[CreateAssetMenu(menuName = "Card Effects/CP Change")]
public class CPEffect : CardEffect
{
    public int amount;

    public override IEnumerator Execute(CardContext context)
    {
        if (amount > 0)
            context.owner.GainCP(amount);
        else
            context.owner.SpendCP(-amount);

        yield return null;
    }
}