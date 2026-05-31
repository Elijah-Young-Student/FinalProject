using System.Collections;
using UnityEngine;

[CreateAssetMenu(menuName = "Card Effects/Damage")]
public class DamageEffect : CardEffect
{
    public int amount;

    public override IEnumerator Execute(CardContext context)
    {
        context.target.TakeDamage(new DamagePacket
        {
            amount = amount,
            target = context.target,
            damageType = DamageType.Normal
        });

        yield return null;
    }
}