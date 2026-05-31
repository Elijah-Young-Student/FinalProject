using UnityEngine;

[CreateAssetMenu(menuName = "TokenEffects/Sneak Attack")]
public class SneakAttackEffect : StatusEffect
{
    public override bool ConsumeOnUse => true;

    public int bonusDamage = 2;

    public override int ModifyOutgoingDamage(CharacterState target, int damage)
    {
        return damage + bonusDamage;
    }
}