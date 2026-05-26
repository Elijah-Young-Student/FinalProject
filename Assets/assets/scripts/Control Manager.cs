using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlManager : MonoBehaviour
{

    public GameObject CPDial;
    private CPDial CPScript;
    public GameObject HealthDial;
    private HealthDial healthScript;
    public GameObject CardManager;
    private CardManager cardManager;
    public GameObject DiceManager;
    private DiceManager diceManager;

    private int health;
    private int cp;


    // Start is called before the first frame update
    void Start()
    {
        CPScript = CPDial.GetComponentInChildren<CPDial>();
        healthScript = HealthDial.GetComponent<HealthDial>();
        cardManager = CardManager.GetComponent<CardManager>();
        diceManager = DiceManager.GetComponent<DiceManager>();

        cp = CPScript.CP;
        health = healthScript.health;

        cardManager.StartGame();
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
    /// Add the inputted amount to the current CPhx xfvgbnfvgbhn
    /// <param name="cpChange">value to add to the CP</param>
    private void ImpactCP(int cpChange)
    {
        cp += cpChange;
        UpdateCP(cp);
    }

    
    private void UpdateCP(int CP)
    {
        CPScript.SetCP(CP);
    }

    private void UpdateHealth(int Health)
    {
        healthScript.SetHealth(Health);
    }

    private bool UpKeep()
    {
        // will loop through a list of all the things that need upkeep
        return true;
    }

    public void IncomePhase()
    {

    }

    public void MainPhase()
    {

    }
    
    public void RollPhaseOffence()
    {

    }

    public void RollPhaseTarget()
    {

    }

    public void RollPhaseDefence()
    {

    }

    public void DiscardPhase()
    {

    }

    public void Passive()
    {

    }
}