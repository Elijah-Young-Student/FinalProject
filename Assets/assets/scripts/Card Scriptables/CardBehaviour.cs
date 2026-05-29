using UnityEngine;

public class CardBehaviour : MonoBehaviour
{
    public int cost;
    public CardEffect[] effects;
    public Card.CardType cardType;

    public void Initialize(GameObject self)
    {
        // optional hook if needed
    }

    public void Play(
        CharacterState player,
        DiceManager diceManager,
        ControlManager controlManager)
    {
        if (!player.SpendCP(cost))
            return;

        foreach (CardEffect effect in effects)
        {
            if (cardType == Card.CardType.Upgrade)
                player.AddEffect(effect);
            else
                effect.OnPlay(player, diceManager, controlManager);
        }
    }
}