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

    public enum GamePhase
    {
        Income,
        Main,
        RollOffence,
        RollTarget,
        RollDefence,
        Discard,
        Passive
    }
    public GamePhase currentPhase;

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
    public void ImpactCP(int cpChange)
    {
        cp += cpChange;
        UpdateCP(cp);
    }

    private void Draw()
    {
        cardManager.DrawCard();
    }

    
    private void UpdateCP(int CP)
    {
        if (cp < 15 && cp > 0)
        {
            CPScript.SetCP(CP);
        }
    }

    private void UpdateHealth(int Health)
    {
        healthScript.SetHealth(Health);
    }

    public bool UpKeep()
    {
        // will loop through a list of all the things that need upkeep
        return true;
    }

    public void IncomePhase()
    {
        Draw();
        ImpactCP(1);
    }

    public void MainPhase()
    {
        currentPhase = GamePhase.Main;

        cardManager.EnableMainPhase(); // ADD THIS CALL
    }

    public void RollPhaseOffence()
    {
        currentPhase = GamePhase.RollOffence;

        diceManager.StartRollPhase(); // ADD THIS CALL
    }

    public void RollPhaseTarget()
    {

    }

    public void RollPhaseDefence()
    {

    }

    public void DiscardPhase()
    {
        currentPhase = GamePhase.Discard;

        cardManager.StartDiscardPhase(); // ADD THIS CALL
    }

    public void Passive()
    {

    }

    public void NextPhase()
    {
        switch (currentPhase)
        {
            case GamePhase.Income:
                MainPhase();
                break;

            case GamePhase.Main:
                RollPhaseOffence();
                break;

            case GamePhase.RollOffence:
                DiscardPhase();
                break;

            case GamePhase.Discard:
                Passive();
                break;

            case GamePhase.Passive:
                IncomePhase();
                break;
        }
    }
}