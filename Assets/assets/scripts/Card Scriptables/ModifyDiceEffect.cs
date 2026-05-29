using UnityEngine;

[CreateAssetMenu(menuName = "Card Effects/Modify Dice")]
public class ModifyDiceEffect : CardEffect
{
    public int numberOfDice = 1;

    public bool allowAnyFace = true;

    public string[] allowedFaces;

    public override void OnPlay(
        CharacterState owner,
        DiceManager diceManager,
        ControlManager controlManager)
    {
        diceManager.StartDiceModification(
            numberOfDice,
            allowAnyFace,
            allowedFaces
        );
    }
}