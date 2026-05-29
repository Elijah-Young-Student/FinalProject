using UnityEngine;

[CreateAssetMenu(menuName = "TokenEffects/Concussion")]
public class ConcussionEffect : StatusEffect
{
    public override bool SkipIncomePhase()
    {
        return true;
    }
}