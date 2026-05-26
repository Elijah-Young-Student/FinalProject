using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CardManager : MonoBehaviour
{
    [Header("Deck + Hand")]
    public List<Card> cardDeck;
    public List<Card> discardPile;
    private List<Card> PlayerHand;

    [Header("UI")]
    public Transform handArea;

    public GameObject actionPanel;

    private GameObject selectedCardObject;

    private Card selectedCard;

    private Transform originalParent;

    public void StartGame()
    {
        actionPanel.SetActive(false);

        // game starts with 4 cards in the hand
        DrawCard();
        DrawCard();
        DrawCard();
        DrawCard();
    }

    public void DrawCard()
    {
        if (cardDeck.Count == 0)
            ShuffleDiscardPileIntoDraw();

        Card card = cardDeck[0];
        cardDeck.RemoveAt(0);
        PlayerHand.Add(card);

        GameObject cardObject = Instantiate(card.cardPrefab, handArea);

        CardDisplay display = cardObject.GetComponent<CardDisplay>();

        display.Setup(card, this);
    }

    public void SelectCard(GameObject cardObj, Card card)
    {
        // Reset old selected card
        if (selectedCardObject != null)
        {
            ReturnCard();
        }

        selectedCardObject = cardObj;

        selectedCard = card;

        originalParent = cardObj.transform.parent;

        // Bring to front
        cardObj.transform.SetParent(transform);

        cardObj.transform.SetAsLastSibling();

        // Move to center
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

        Debug.Log("Played " + selectedCard.cardName);

        // DO CARD EFFECT HERE
        // selectedCard.CardOutcomes(...);

        PlayerHand.Remove(selectedCard);

        Destroy(selectedCardObject);

        selectedCardObject = null;

        selectedCard = null;

        actionPanel.SetActive(false);
    }

    public void Nevermind()
    {
        ReturnCard();

        selectedCardObject = null;

        selectedCard = null;

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

    public void ShuffleDiscardPileIntoDraw()
    {
        cardDeck.AddRange(discardPile);
        discardPile.Clear();
        ShuffleDeck();
    }

    public void ShuffleDeck()
    {
        for (int i = cardDeck.Count - 1; i > 0; i--)
        {
            int randomIndex = UnityEngine.Random.Range(0, i + 1);

            // Swap the items
            Card temp = cardDeck[i];
            cardDeck[i] = cardDeck[randomIndex];
            cardDeck[randomIndex] = temp;
        }
    }
}