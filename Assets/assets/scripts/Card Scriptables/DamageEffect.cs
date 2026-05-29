using UnityEngine;

[CreateAssetMenu(menuName = "Card Effects/Damage")]
public class DamageEffect : CardEffect
{
    public int bonusDamage;

    public override void OnApply(CharacterState owner)
    {
        owner.outgoingDamageBonus += bonusDamage;
    }

    public override void OnRemove(CharacterState owner)
    {
        owner.outgoingDamageBonus -= bonusDamage;
    }
}