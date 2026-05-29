using UnityEngine;

[CreateAssetMenu(menuName = "TokenEffects/Poison")]
public class PoisonEffect : StatusEffect
{
    public int damagePerStack = 1;

    public override void OnTick(CharacterState target)
    {
        int stacks =
            target.GetStatusStacks(statusName);

        int totalDamage =
            stacks * damagePerStack;

        DamagePacket packet = new DamagePacket
        {
            amount = totalDamage,
            damageType = DamageType.Pure,
            source = null,
            target = target,
            offensiveRollDamage = false
        };

        target.TakeDamage(packet);

        Debug.Log(
            target.name +
            " takes " +
            totalDamage +
            " poison damage.");
    }
}