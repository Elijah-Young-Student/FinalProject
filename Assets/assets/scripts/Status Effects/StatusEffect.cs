using UnityEngine;

public abstract class StatusEffect : ScriptableObject
{
    [Header("Info")]
    public string statusName;

    public Sprite icon;

    [TextArea]
    public string description;

    [Header("Defaults")]
    public int defaultDuration = 1;

    public int maxStacks = 1;

    // Called when first applied
    public virtual void OnApply(CharacterState target)
    {

    }

    // Called every turn/phase if needed
    public virtual void OnTick(CharacterState target)
    {

    }

    // Called when removed
    public virtual void OnRemove(CharacterState target)
    {

    }

    // Optional hooks
    public virtual int ModifyIncomingDamage(CharacterState target, int damage)
    {
        return damage;
    }

    public virtual int ModifyOutgoingDamage(CharacterState target, int damage)
    {
        return damage;
    }

    public virtual bool SkipIncomePhase()
    {
        return false;
    }

    public virtual bool PreventsActions()
    {
        return false;
    }

        public virtual bool PreventsOffensiveRollDamage()
    {
        return false;
    }
}