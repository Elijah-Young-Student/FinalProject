using System.Collections.Generic;
using UnityEngine;

public class CharacterState : MonoBehaviour
{
    [Header("Stats")]
    public int health = 50;
    public int cp = 2;

    [Header("Runtime Effects")]
    public List<CardEffect> activeEffects = new();

    [Header("Combat Modifiers")]
    public int outgoingDamageBonus = 0;
    public int incomingDamageReduction = 0;

    [Header("Statuses")]
    public List<StatusInstance> statuses = new();

    public void GainCP(int amount)
    {
        cp += amount;
    }

    public bool SpendCP(int amount)
    {
        if (cp < amount)
            return false;

        cp -= amount;
        return true;
    }

    public void TakeDamage(DamagePacket packet)
    {
        // STATUS MODIFIERS

        foreach (StatusInstance status in statuses)
        {
            if (
                packet.offensiveRollDamage &&
                status.effect.PreventsOffensiveRollDamage()
            )
            {
                Debug.Log("Damage prevented.");

                return;
            }
        }

        foreach (StatusInstance status in statuses)
        {
            packet.amount =
                status.effect.ModifyIncomingDamage(
                    this,
                    packet.amount);
        }

        // PURE DAMAGE IGNORES REDUCTION

        if (packet.damageType != DamageType.Pure)
        {
            packet.amount -= incomingDamageReduction;
        }

        if (packet.amount < 0)
            packet.amount = 0;

        health -= packet.amount;

        Debug.Log(
            name +
            " took " +
            packet.amount +
            " " +
            packet.damageType +
            " damage.");
    }

    public int ModifyOutgoingDamage(int baseDamage)
    {
        int damage = baseDamage + outgoingDamageBonus;

        foreach (StatusInstance status in statuses)
        {
            damage = status.effect.ModifyOutgoingDamage(
                this,
                damage);
        }

        return damage;
    }

    public void AddEffect(CardEffect effect)
    {
        activeEffects.Add(effect);
        effect.OnApply(this);
    }

    public void RemoveEffect(CardEffect effect)
    {
        effect.OnRemove(this);
        activeEffects.Remove(effect);
    }

    public void RemoveStatus(StatusEffect effect)
    {
        for (int i = statuses.Count - 1; i >= 0; i--)
        {
            if (statuses[i].effect == effect)
            {
                effect.OnRemove(this);

                statuses.RemoveAt(i);
            }
        }
    }

    public void RemoveStatusByName(string statusName)
    {
        for (int i = statuses.Count - 1; i >= 0; i--)
        {
            if (statuses[i].effect.statusName == statusName)
            {
                statuses[i].effect.OnRemove(this);

                statuses.RemoveAt(i);
            }
        }
    }

    public bool HasStatus(string statusName)
    {
        foreach (StatusInstance status in statuses)
        {
            if (status.effect.statusName == statusName)
                return true;
        }

        return false;
    }

    public int GetStatusStacks(string statusName)
    {
        foreach (StatusInstance status in statuses)
        {
            if (status.effect.statusName == statusName)
                return status.stacks;
        }

        return 0;
    }

    public void AddStatus(StatusEffect effect, int stacks = 1)
    {
        foreach (StatusInstance status in statuses)
        {
            if (status.effect == effect)
            {
                status.stacks += stacks;

                status.stacks = Mathf.Min(
                    status.stacks,
                    effect.maxStacks);

                status.duration = Mathf.Max(
                    status.duration,
                    effect.defaultDuration);

                return;
            }
        }

        StatusInstance newStatus = new StatusInstance
        {
            effect = effect,
            stacks = stacks,
            duration = effect.defaultDuration
        };

        statuses.Add(newStatus);

        effect.OnApply(this);
    }

    public bool HasActionBlockingStatus()
    {
        foreach (StatusInstance status in statuses)
        {
            if (status.effect.PreventsActions())
                return true;
        }

        return false;
    }
}