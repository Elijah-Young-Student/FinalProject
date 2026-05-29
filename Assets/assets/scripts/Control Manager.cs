using UnityEngine;

public class ControlManager : MonoBehaviour
{
    [Header("References")]
    public GameObject CPDial;
    public CPDial CPScript;

    public GameObject HealthDial;
    public HealthDial healthScript;

    public CardManager cardManager;

    public DiceManager diceManager;

    public CharacterState playerState;

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

    void Start()
    {
        CPScript = CPDial.GetComponentInChildren<CPDial>();
        healthScript = HealthDial.GetComponent<HealthDial>();

        CPScript.SetCP(playerState.cp);
        healthScript.SetHealth(playerState.health);

        UpdateUI();

        cardManager.StartGame();

        currentPhase = GamePhase.Income;
    }

    public void Damage(
    int dmg,
    DamageType type,
    bool offensiveRollDamage = false)
{
    DamagePacket packet = new DamagePacket
    {
        amount = dmg,
        damageType = type,
        source = null,
        target = playerState,
        offensiveRollDamage = offensiveRollDamage
    };

    playerState.TakeDamage(packet);

    UpdateUI();
}

    public void ImpactCP(int cpChange)
    {
        playerState.GainCP(cpChange);
        UpdateUI();
    }

    public bool SpendCP(int amount)
    {
        bool success = playerState.SpendCP(amount);

        UpdateUI();

        return success;
    }

    private void Draw()
    {
        cardManager.DrawCard();
    }

    private void UpdateUI()
    {
        CPScript.SetCP(playerState.cp);
        healthScript.SetHealth(playerState.health);
    }

    public void IncomePhase()
    {
        currentPhase = GamePhase.Income;

        foreach (StatusInstance status in playerState.statuses)
        {
            if (status.effect is ConcussionEffect)
            {
                Debug.Log("Income phase skipped due to Concussion.");

                playerState.RemoveStatus(status.effect);

                MainPhase();

                return;
            }
        }

        Draw();

        ImpactCP(1);
    }

    public void MainPhase()
    {
        currentPhase = GamePhase.Main;

        // cardManager.EnableMainPhase();
    }

    public void RollPhaseOffence()
    {
        currentPhase = GamePhase.RollOffence;


        diceManager.StartRollPhase();
    }

    public void RollPhaseTarget()
    {
        currentPhase = GamePhase.RollTarget;
    }

    public void RollPhaseDefence()
    {
        currentPhase = GamePhase.RollDefence;

        if (playerState.HasActionBlockingStatus())
        {
            Debug.Log("Player is stunned.");
            return;
        }
    }

    public void DiscardPhase()
    {
        currentPhase = GamePhase.Discard;

        cardManager.StartDiscardPhase();
    }

    public void Passive()
    {
        currentPhase = GamePhase.Passive;
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