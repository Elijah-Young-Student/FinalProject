using System.Collections;
using UnityEngine;

[CreateAssetMenu(menuName = "Card Effects/Remove Status")]
public class RemoveStatusEffect : CardEffect
{
    public override IEnumerator Execute(CardContext context)
    {
        bool waiting = true;
        StatusEffect selectedStatus = null;

        StatusEffectManager.Instance.StartSelection(
            status =>
            {
                selectedStatus = status;
                waiting = false;
            });

        while (waiting)
            yield return null;

        if (selectedStatus != null)
        {
            context.target.RemoveStatus(selectedStatus);
        }
    }
}