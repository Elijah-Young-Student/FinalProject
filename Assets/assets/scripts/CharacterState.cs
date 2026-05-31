using System.Collections.Generic;
using UnityEngine;

public class CharacterState : MonoBehaviour
{
    [Header("Stats")]
    public int maxHealth = 99;
    public int health = 50;

    public int maxCp = 15;
    public int cp = 2;

    [Header("Combat Modifiers")]
    public int outgoingDamageBonus = 0;
    public int incomingDamageReduction = 0;

    [Header("Statuses")]
    public List<StatusInstance> statuses = new();

    private Dictionary<StatusEffect, GameObject> statusTokens = new();

    [Header("Board Reference")]
    public GameObject statusAnchor;

    public CardManager cardManager;

    #region CP

    public void GainCP(int amount)
    {
        cp = Mathf.Clamp(cp + amount, 0, maxCp);
    }

    public bool SpendCP(int amount)
    {
        if (cp < amount)
            return false;

        cp -= amount;
        return true;
    }

    #endregion

    #region HEALTH

    public void Heal(int amount)
    {
        health = Mathf.Clamp(health + amount, 0, maxHealth);
    }

    public void TakeDamage(DamagePacket packet)
    {
        // STATUS PREVENTION
        foreach (var status in statuses)
        {
            if (packet.offensiveRollDamage &&
                status.effect.PreventsOffensiveRollDamage())
            {
                Debug.Log("Damage prevented by status.");
                return;
            }
        }

        // MODIFY INCOMING DAMAGE
        foreach (var status in statuses)
        {
            packet.amount = status.effect.ModifyIncomingDamage(this, packet.amount);
        }

        // ARMOR / REDUCTION
        if (packet.damageType != DamageType.Pure)
            packet.amount -= incomingDamageReduction;

        packet.amount = Mathf.Max(0, packet.amount);

        health -= packet.amount;

        Debug.Log($"{name} took {packet.amount} damage.");
    }

    public int ModifyOutgoingDamage(int baseDamage)
    {
        int damage = baseDamage + outgoingDamageBonus;

        for (int i = statuses.Count - 1; i >= 0; i--)
        {
            damage = statuses[i].effect.ModifyOutgoingDamage(this, damage);

            if (statuses[i].effect.ConsumeOnUse)
            {
                statuses[i].stacks--;

                if (statuses[i].stacks <= 0)
                    RemoveStatus(statuses[i].effect);
            }
        }

        return damage;
    }

    #endregion

    #region STATUS SYSTEM

    public void AddStatus(StatusEffect effect, int stacks = 1)
    {
        for (int i = 0; i < statuses.Count; i++)
        {
            if (statuses[i].effect == effect)
            {
                statuses[i].stacks += stacks;
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

        SpawnStatusToken(newStatus);
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
        foreach (var status in statuses)
            if (status.effect.statusName == statusName)
                return true;

        return false;
    }

    public int GetStatusStacks(string statusName)
    {
        foreach (var status in statuses)
            if (status.effect.statusName == statusName)
                return status.stacks;

        return 0;
    }

    public bool HasActionBlockingStatus()
    {
        foreach (var status in statuses)
            if (status.effect.PreventsActions())
                return true;

        return false;
    }

    #endregion

    #region VISUALS

    private void SpawnStatusToken(StatusInstance status)
    {
        if (status.effect.tokenPrefab == null)
            return;

        GameObject token = Instantiate(
            status.effect.tokenPrefab,
            statusAnchor.transform.position,
            Quaternion.identity,
            statusAnchor.transform
        );

        status.tokenObject = token;

        StatusToken statusToken = token.GetComponent<StatusToken>();
        if (statusToken != null)
            statusToken.effect = status.effect;

        Vector3 offset = new Vector3(
            Random.Range(-5f, 5f),
            0f,
            Random.Range(-5f, 5f)
        );

        Vector3 target = statusAnchor.transform.position + offset;

        if (StatusEffectManager.Instance != null)
        {
            StatusEffectManager.Instance.StartCoroutine(
                StatusEffectManager.Instance.MoveToken(token, target, 50f)
            );
        }
    }

    #endregion
}