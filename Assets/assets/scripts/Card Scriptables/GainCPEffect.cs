using UnityEngine;

[CreateAssetMenu(menuName = "Card Effects/Gain CP")]
public class GainCPEffect : CardEffect
{
    public int amount;

    public override void OnPlay(
        CharacterState owner,
        DiceManager diceManager,
        ControlManager controlManager)
    {
        owner.GainCP(amount);
    }
}