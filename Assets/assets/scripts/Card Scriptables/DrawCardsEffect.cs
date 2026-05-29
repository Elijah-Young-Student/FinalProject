using UnityEngine;

[CreateAssetMenu(menuName = "Card Effects/Draw Cards")]
public class DrawCardsEffect : CardEffect
{
    public int cardsToDraw = 1;

    public override void OnPlay(
        CharacterState owner,
        DiceManager diceManager,
        ControlManager controlManager)
    {
        for (int i = 0; i < cardsToDraw; i++)
        {
            controlManager.cardManager.DrawCard();
        }
    }
}