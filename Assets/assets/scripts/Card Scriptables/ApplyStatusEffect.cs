using System.Collections;
using UnityEngine;

[CreateAssetMenu(menuName = "Card Effects/Apply Status")]
public class ApplyStatusEffect : CardEffect
{
    public StatusEffect status;
    public int stacks = 1;

    public override IEnumerator Execute(CardContext context)
    {
        context.target.AddStatus(status, stacks);
        yield return null;
    }
}