using UnityEngine;

[CreateAssetMenu(menuName = "TokenEffects/Poison")]
public class PoisonEffect : StatusEffect
{
    public int damagePerStack = 1;

    public override void OnTick(CharacterState target)
    {
        int stacks = target.GetStatusStacks(statusName);

        if (stacks <= 0)
            return;

        int totalDamage = stacks * damagePerStack;

        DamagePacket packet = new DamagePacket
        {
            amount = totalDamage,
            damageType = DamageType.Pure,
            source = null,
            target = target,
            offensiveRollDamage = false
        };

        target.TakeDamage(packet);
    }
}