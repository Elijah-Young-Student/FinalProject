using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardManager : MonoBehaviour
{
    [Header("Deck + Hand (GAMEOBJECTS ONLY)")]
    public List<GameObject> cardDeck = new();

    public List<GameObject> discardPile = new();

    public List<GameObject> PlayerHand = new();

    [Header("References")]
    public ControlManager controlManager;

    public DiceManager diceManager;

    [Header("UI")]
    public Transform handArea;

    public GameObject actionPanel;

    public GameObject discardBanner;

    private GameObject selectedCardObject;

    private CardBehaviour selectedCard;

    private Transform originalParent;

    private bool isDiscarding = false;

    void Update()
    {
        if (controlManager.currentPhase != ControlManager.GamePhase.Main)
            return;
    }

    public void StartDiscardPhase()
    {
        isDiscarding = true;
    }

    public void StartGame()
    {
        actionPanel.SetActive(false);

        DrawCard();
        DrawCard();
        DrawCard();
        DrawCard();
    }

    public void DrawCard()
    {
        if (cardDeck.Count == 0)
        {
            ShuffleDiscardIntoDeck();
        }

        if (cardDeck.Count == 0)
            return;

        GameObject cardObj = cardDeck[0];
        cardDeck.RemoveAt(0);

        PlayerHand.Add(cardObj);

        GameObject spawned =
            Instantiate(cardObj, handArea);

        // IMPORTANT: cardObj is prefab template,
        // spawned is runtime instance

        CardBehaviour behaviour =
            spawned.GetComponent<CardBehaviour>();

        behaviour.Initialize(spawned);

        CardDisplay display =
            spawned.GetComponent<CardDisplay>();

        if (display != null)
            display.Setup(spawned, this);
    }

    public void SelectCard(GameObject cardObj)
    {
        if (isDiscarding)
        {
            DiscardCard(cardObj);
            return;
        }

        if (selectedCardObject != null)
            ReturnCard();

        selectedCardObject = cardObj;

        selectedCard =
            cardObj.GetComponent<CardBehaviour>();

        originalParent = cardObj.transform.parent;

        cardObj.transform.SetParent(transform);
        cardObj.transform.SetAsLastSibling();

        RectTransform rect =
            cardObj.GetComponent<RectTransform>();

        rect.anchoredPosition = Vector2.zero;
        rect.localScale = Vector3.one * 1.25f;

        actionPanel.SetActive(true);
    }

    public void PlayCard()
    {
        if (selectedCard == null)
            return;

        CharacterState player =
            controlManager.playerState;

        selectedCard.Play(
            player,
            diceManager,
            controlManager);

        PlayerHand.Remove(selectedCardObject);
        discardPile.Add(selectedCardObject);

        Destroy(selectedCardObject);

        selectedCard = null;
        selectedCardObject = null;

        actionPanel.SetActive(false);
    }

    public void Nevermind()
    {
        ReturnCard();

        selectedCard = null;
        selectedCardObject = null;

        actionPanel.SetActive(false);
    }

    void ReturnCard()
    {
        if (selectedCardObject == null)
            return;

        selectedCardObject.transform.SetParent(originalParent);

        RectTransform rect =
            selectedCardObject.GetComponent<RectTransform>();

        rect.localScale = Vector3.one;
    }

    public void DiscardCard(GameObject cardObj)
    {
        PlayerHand.Remove(cardObj);
        discardPile.Add(cardObj);

        Destroy(cardObj);

        LayoutRebuilder.ForceRebuildLayoutImmediate(
            handArea.GetComponent<RectTransform>()
        );

        if (PlayerHand.Count <= 6)
        {
            isDiscarding = false;
            discardBanner.SetActive(false);
        }
    }

    public void ShuffleDiscardIntoDeck()
    {
        cardDeck.AddRange(discardPile);
        discardPile.Clear();

        ShuffleDeck();
    }

    public void ShuffleDeck()
    {
        for (int i = cardDeck.Count - 1; i > 0; i--)
        {
            int r = Random.Range(0, i + 1);

            GameObject temp = cardDeck[i];
            cardDeck[i] = cardDeck[r];
            cardDeck[r] = temp;
        }
    }
}