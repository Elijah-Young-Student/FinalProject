using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlManager : MonoBehaviour
{

    public GameObject CPDial;
    private CPDial CPScript;
    public GameObject HealthDial;
    private HealthDial healthScript;
    public CardManager cardManager;

    private int health;
    private int cp;

    // Start is called before the first frame update
    void Start()
    {
        CPScript = CPDial.GetComponentInChildren<CPDial>();
        healthScript = HealthDial.GetComponent<HealthDial>();

        cp = CPScript.CP;
        health = healthScript.health;

        if (!CPScript)
            Debug.LogError("There is no CPDial or CPDial script");
        if (!healthScript)
            Debug.LogError("There is no CPDial or CPDial script");
    }

    /// <summary>
    /// subtract the inputted amount from current health
    /// </summary>
    /// <param name="dmg">the amount of damage to take from the player</param>
    public void Damage(int dmg)
    {
        health -= dmg;
        UpdateHealth(health);
    }

    /// <summary>
    /// Add the inputted amount to the current CP
    /// </summary>
    /// <param name="cpChange">value to add to the CP</param>
    private void ImpactCP(int cpChange)
    {
        cp += cpChange;
        UpdateCP(cp);
    }

    public void StartTurn()
    {
        if (UpKeep())
        {
            ImpactCP(1);
        }
    }

    private bool UpKeep()
    {
        // will loop through a list of all the things that need upkeep
        return true;
    }

    private void UpdateCP(int CP)
    {
        CPScript.SetCP(CP);
    }

    private void UpdateHealth(int Health)
    {
        healthScript.SetHealth(Health);
    }
}
