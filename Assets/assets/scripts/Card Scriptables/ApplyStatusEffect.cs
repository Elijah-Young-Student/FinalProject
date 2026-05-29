using UnityEngine;

[CreateAssetMenu(menuName = "Card Effects/Apply Status")]
public class ApplyStatusEffect : CardEffect
{
    public StatusEffect statusToApply;

    public int stacks = 1;

    public override void OnPlay(
        CharacterState owner,
        DiceManager diceManager,
        ControlManager controlManager)
    {
        owner.AddStatus(statusToApply, stacks);
    }
}