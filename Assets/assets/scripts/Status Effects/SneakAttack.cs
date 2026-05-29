using UnityEngine;

[CreateAssetMenu(menuName = "TokenEffects/Sneak Attack")]
public class SneakAttackEffect : StatusEffect
{
    public int bonusDamage = 2;

    public override int ModifyOutgoingDamage(
        CharacterState target,
        int damage)
    {
        target.RemoveStatus(this);

        return damage + bonusDamage;
    }
}