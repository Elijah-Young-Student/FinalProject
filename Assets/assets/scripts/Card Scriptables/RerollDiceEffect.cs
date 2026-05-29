using UnityEngine;

[CreateAssetMenu(menuName = "Card Effects/Reroll Dice")]
public class RerollDiceEffect : CardEffect
{
    public int rerollAmount = 1;

    public override void OnPlay(
        CharacterState owner,
        DiceManager diceManager,
        ControlManager controlManager)
    {
        diceManager.EnableBonusRerolls(rerollAmount);
    }
}