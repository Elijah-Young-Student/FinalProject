using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Burst.CompilerServices;
using Unity.VisualScripting.Dependencies.Sqlite;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ControlManager : MonoBehaviour
{
    [Header("UI References")]
    public GameObject CPDial;
    public GameObject HealthDial;

    [Header("Managers")]
    public CardExecutor cardExecutor;
    public DiceManager diceManager;
    public CardManager cardManager;

    [Header("Players")]
    public CharacterState playerState;
    public CharacterState opponentState;

    [Header("UI")]
    public GameObject playerSelectionPanel;

    public GraphicRaycaster uiRaycaster;
    public EventSystem eventSystem;

    [Header("State")]
    public GamePhase currentPhase = GamePhase.Income;

    public CharacterState activePlayer;
    public CharacterState inactivePlayer;

    public CharacterState selectedTarget;

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

    void Start()
    {
        activePlayer = playerState;
        inactivePlayer = opponentState;

        StartCoroutine(GameLoop());
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            TrySelectCard();
        }
    }

    private void TrySelectCard()
    {
        PointerEventData pointer = new PointerEventData(eventSystem);
        pointer.position = Input.mousePosition;

        List<RaycastResult> results = new List<RaycastResult>();
        uiRaycaster.Raycast(pointer, results);

        foreach (var result in results)
        {
            CardView view = result.gameObject.GetComponentInParent<CardView>();

            if (view != null)
            {
                cardManager.SelectCard(view);
                return;
            }
        }
    }

    // =========================================================
    // MAIN GAME LOOP
    // =========================================================

    private IEnumerator GameLoop()
    {
        while (true)
        {
            yield return IncomePhase();
            yield return MainPhase();
            yield return RollOffencePhase();
            yield return RollTargetPhase();
            yield return RollDefencePhase();
            yield return DiscardPhase();

            SwapPlayers();
        }
    }

    private void SwapPlayers()
    {
        (activePlayer, inactivePlayer) = (inactivePlayer, activePlayer);
    }

    // =========================================================
    // PHASE SKIP HELPERS
    // =========================================================

    private bool SkipIncome(CharacterState player)
    {
        foreach (var s in player.statuses)
            if (s.effect.SkipIncomePhase())
                return true;

        return false;
    }

    private bool SkipRoll(CharacterState player)
    {
        foreach (var s in player.statuses)
            if (s.effect.PreventsActions())
                return true;

        return false;
    }

    // =========================================================
    // PHASES
    // =========================================================

    private IEnumerator IncomePhase()
    {
        currentPhase = GamePhase.Income;

        if (!SkipIncome(activePlayer))
        {
            activePlayer.GainCP(1);

            foreach (var status in activePlayer.statuses)
                status.effect.OnTick(activePlayer);
        }
        else
        {
            Debug.Log("Income phase skipped.");
        }

        yield return null;
    }

    private IEnumerator MainPhase()
    {
        currentPhase = GamePhase.Main;

        while (currentPhase == GamePhase.Main)
            yield return null;
    }

    private IEnumerator RollOffencePhase()
    {
        currentPhase = GamePhase.RollOffence;

        if (!SkipRoll(activePlayer))
        {
            diceManager.StartFullRoll();

            while (diceManager.IsRolling)
                yield return null;
        }
        else
        {
            Debug.Log("Offence roll skipped.");
        }
    }

    private IEnumerator RollTargetPhase()
    {
        currentPhase = GamePhase.RollTarget;

        if (!SkipRoll(inactivePlayer))
        {
            diceManager.StartFullRoll();

            while (diceManager.IsRolling)
                yield return null;
        }
        else
        {
            Debug.Log("Target roll skipped.");
        }
    }

    private IEnumerator RollDefencePhase()
    {
        currentPhase = GamePhase.RollDefence;

        if (!SkipRoll(inactivePlayer))
        {
            diceManager.StartFullRoll();

            while (diceManager.IsRolling)
                yield return null;
        }
        else
        {
            Debug.Log("Defence roll skipped.");
        }
    }

    private IEnumerator DiscardPhase()
    {
        currentPhase = GamePhase.Discard;
        yield return null;
    }

    // =========================================================
    // TARGET SELECTION
    // =========================================================

    public IEnumerator SelectTarget(Action<CharacterState> onSelected)
    {
        selectedTarget = null;

        playerSelectionPanel.SetActive(true);

        while (selectedTarget == null)
        {
            if (Input.GetMouseButtonDown(0))
            {
                PointerEventData data = new PointerEventData(eventSystem);
                data.position = Input.mousePosition;

                List<RaycastResult> results = new();
                uiRaycaster.Raycast(data, results);

                foreach (var r in results)
                {
                    TextMeshProUGUI tmp = r.gameObject.GetComponentInParent<TextMeshProUGUI>();

                    if (tmp == null) continue;

                    if (tmp.text == "You")
                    {
                        selectedTarget = playerState;
                        break;
                    }

                    if (tmp.text == "Opponent")
                    {
                        selectedTarget = opponentState;
                        break;
                    }
                }
            }

            yield return null;
        }

        playerSelectionPanel.SetActive(false);

        onSelected?.Invoke(selectedTarget);
    }

    // =========================================================
    // CARD EXECUTION ENTRY POINT
    // =========================================================

    public void PlayCard(Card card, CharacterState owner)
    {
        StartCoroutine(PlayCardRoutine(card, owner));
    }

    private IEnumerator PlayCardRoutine(Card card, CharacterState owner)
    {
        CharacterState target = opponentState;

        if (RequiresTargetSelection(card))
        {
            yield return SelectTarget(t => target = t);
        }

        yield return cardExecutor.ExecuteCard(
            card,
            owner,
            target,
            diceManager,
            this
        );

        cardManager?.DiscardCardAfterPlay(owner);
    }

    private bool RequiresTargetSelection(Card card)
    {
        return true;
    }

    // =========================================================
    // WRAPPERS
    // =========================================================

    public void Damage(CharacterState target, DamagePacket packet)
    {
        target.TakeDamage(packet);
    }

    public void Heal(CharacterState target, int amount)
    {
        target.Heal(amount);
    }

    public void GainCP(CharacterState target, int amount)
    {
        target.GainCP(amount);
    }

    public bool SpendCP(CharacterState target, int amount)
    {
        return target.SpendCP(amount);
    }
}