using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardManager : MonoBehaviour
{
    [Header("Deck System")]
    public List<Card> deck = new();
    public List<Card> discardPile = new();

    [Header("Hand Runtime")]
    public List<CardView> hand = new();

    [Header("UI")]
    public Transform handArea;
    public GameObject actionPanel;

    [Header("References")]
    public ControlManager controlManager;
    public CardExecutor cardExecutor;

    private CardView selectedCard;

    private const int MAX_HAND_SIZE = 6;

    public void StartGame(int startingCards = 4)
    {
        ShuffleDeck();

        for (int i = 0; i < startingCards; i++)
            DrawCard();
    }

    public void DrawCard()
    {
        if (deck.Count == 0)
            ReshuffleDiscardIntoDeck();

        if (deck.Count == 0)
            return;

        Card drawnCard = deck[0];
        deck.RemoveAt(0);

        GameObject obj = new GameObject(drawnCard.cardName);
        obj.transform.SetParent(handArea);

        CardView view = obj.AddComponent<CardView>();
        view.Initialize(drawnCard, this);

        hand.Add(view);

        EnforceHandLimit();
    }

    public void SelectCard(CardView card)
    {
        if (selectedCard == card)
        {
            DeselectCard();
            return;
        }

        DeselectCard();

        selectedCard = card;

        selectedCard.transform.SetAsLastSibling();
        selectedCard.transform.localScale = Vector3.one * 1.2f;

        actionPanel.SetActive(true);
    }

    public void DeselectCard()
    {
        if (selectedCard == null)
            return;

        selectedCard.transform.localScale = Vector3.one;
        selectedCard = null;

        actionPanel.SetActive(false);
    }

    public void PlaySelectedCard()
    {
        if (selectedCard == null)
            return;

        Card card = selectedCard.card;

        if (!CanPlay(card))
        {
            Debug.Log("Card not playable in current phase.");
            return;
        }

        CharacterState owner = controlManager.playerState;
        CharacterState target = ResolveTarget(card);

        StartCoroutine(ExecuteCardRoutine(card, owner, target));
    }

    private IEnumerator ExecuteCardRoutine(Card card, CharacterState owner, CharacterState target)
    {
        yield return cardExecutor.ExecuteCard(
            card,
            owner,
            target,
            controlManager.diceManager,
            controlManager
        );

        DiscardCard(selectedCard);

        selectedCard = null;
        actionPanel.SetActive(false);
    }

    private CharacterState ResolveTarget(Card card)
    {
        return controlManager.opponentState;
    }

    private bool CanPlay(Card card)
    {
        var phase = controlManager.currentPhase;

        switch (card.actionType)
        {
            case Card.ActionType.Instant:
                return true;

            case Card.ActionType.MainPhase:
                return phase == ControlManager.GamePhase.Main;

            case Card.ActionType.RollPhase:
                return phase == ControlManager.GamePhase.RollOffence ||
                       phase == ControlManager.GamePhase.RollTarget ||
                       phase == ControlManager.GamePhase.RollDefence;
        }

        return false;
    }

    public void DiscardCard(CardView view)
    {
        if (view == null) return;

        hand.Remove(view);
        discardPile.Add(view.card);

        Destroy(view.gameObject);
    }

    private void EnforceHandLimit()
    {
        while (hand.Count > MAX_HAND_SIZE)
        {
            DiscardCard(hand[0]);
        }
    }

    private void ReshuffleDiscardIntoDeck()
    {
        deck.AddRange(discardPile);
        discardPile.Clear();
        ShuffleDeck();
    }

    private void ShuffleDeck()
    {
        for (int i = deck.Count - 1; i > 0; i--)
        {
            int r = Random.Range(0, i + 1);
            (deck[i], deck[r]) = (deck[r], deck[i]);
        }
    }

    public void DiscardCardAfterPlay(CharacterState owner)
    {
        // optional hook if you later separate upgrade rules
    }
}