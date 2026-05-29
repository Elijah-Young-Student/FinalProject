using UnityEngine;

[CreateAssetMenu(menuName = "TokenEffects/Stun")]
public class StunEffect : StatusEffect
{
    public override bool PreventsActions()
    {
        return true;
    }
}