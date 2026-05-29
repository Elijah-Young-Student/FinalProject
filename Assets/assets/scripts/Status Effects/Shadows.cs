using UnityEngine;

[CreateAssetMenu(menuName = "TokenEffects/Shadows")]
public class ShadowsEffect : StatusEffect
{
    public override bool PreventsOffensiveRollDamage()
    {
        return true;
    }
}