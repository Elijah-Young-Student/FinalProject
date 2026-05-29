using UnityEngine;

public abstract class CardEffect : ScriptableObject
{
    public virtual void OnApply(CharacterState owner) { }

    public virtual void OnRemove(CharacterState owner) { }

    public virtual void OnPlay(
        CharacterState owner,
        DiceManager diceManager,
        ControlManager controlManager)
    {

    }
}