using System.Collections;
using UnityEngine;

[CreateAssetMenu(menuName = "Card Effects/Transfer Status")]
public class TransferStatusEffect : CardEffect
{
    public StatusEffect status;

    public override IEnumerator Execute(CardContext context)
    {
        if (!context.target.HasStatus(status.statusName))
            yield break;

        context.target.RemoveStatus(status);

        context.owner.AddStatus(status);

        yield return null;
    }
}