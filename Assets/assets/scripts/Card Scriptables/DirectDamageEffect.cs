using UnityEngine;

[CreateAssetMenu(menuName = "Card Effects/Direct Damage")]
public class DirectDamageEffect : CardEffect
{
    public int damage;

    public DamageType damageType;

    public override void OnPlay(
        CharacterState owner,
        DiceManager diceManager,
        ControlManager controlManager)
    {
        int finalDamage =
            owner.ModifyOutgoingDamage(damage);

        DamagePacket packet = new DamagePacket
        {
            amount = finalDamage,
            damageType = damageType,
            source = owner,
            target = owner,
            offensiveRollDamage = true
        };

        owner.TakeDamage(packet);
    }
}