using System.Collections;
using UnityEngine;

public class CardExecutor : MonoBehaviour
{
    public IEnumerator ExecuteCard(
        Card card,
        CharacterState owner,
        CharacterState target,
        DiceManager diceManager,
        ControlManager controlManager)
    {
        if (!owner.SpendCP(card.cost))
            yield break;

        CardContext context = new CardContext
        {
            owner = owner,
            target = target,
            diceManager = diceManager,
            controlManager = controlManager
        };

        foreach (CardEffect effect in card.effects)
        {
            yield return effect.Execute(context);
        }
    }
}