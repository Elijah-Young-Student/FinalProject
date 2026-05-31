using System.Collections;
using UnityEngine;

[CreateAssetMenu(menuName = "Card Effects/Heal")]
public class HealEffect : CardEffect
{
    public int amount;

    public override IEnumerator Execute(CardContext context)
    {
        context.owner.Heal(amount);
        yield return null;
    }
}