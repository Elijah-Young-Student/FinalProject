using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using static ControlManager;

public class CardManager : MonoBehaviour
{
    [Header("Deck + Hand")]
    public List<Card> cardDeck;
    public List<Card> discardPile;
    public List<Card> PlayerHand;

    [Header("UI")]
    public Transform handArea;

    public GameObject actionPanel;

    private GameObject selectedCardObject;

    private Card selectedCard;

    private Transform originalParent;

    public GameObject discardBanner;

    private bool isDiscarding = false;

    void Update()
    {
        if (FindObjectOfType<ControlManager>().currentPhase != GamePhase.Main)
            return;

        // card input logic here
    }

    public void EnableMainPhase()
    {
        // allow card selection + playing
    }

    public void StartDiscardPhase()
    {
        if (PlayerHand.Count <= 6)
            return;

        isDiscarding = true;
        discardBanner.SetActive(true);
        HighlightHand(true);
    }

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
        // DISCARD MODE
        if (isDiscarding)
        {
            DiscardCard(cardObj, card);
            return;
        }

        // NORMAL MODE
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

        // ACTIVATE EFFECT
        //if (selectedCard.effect != null)
        //{
        //    selectedCard.effect.Activate(this);
        //}

        PlayerHand.Remove(selectedCard);

        discardPile.Add(selectedCard);

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

    public void DiscardPhase()
    {
        if (PlayerHand.Count <= 6)
            return;
        isDiscarding = true;
        actionPanel.SetActive(false);
        discardBanner.SetActive(true);
        HighlightHand(true);
    }

    public void DiscardCard(GameObject cardObj, Card card)
    {
        Debug.Log("Discarded " + card.cardName);

        PlayerHand.Remove(card);

        discardPile.Add(card);

        Destroy(cardObj);

        LayoutRebuilder.ForceRebuildLayoutImmediate(
         handArea.GetComponent<RectTransform>()
        );

        // Finished discarding
        if (PlayerHand.Count <= 6)
        {
            isDiscarding = false;

            discardBanner.SetActive(false);

            HighlightHand(false);

            Debug.Log("Discard phase complete.");
        }
    }

    void HighlightHand(bool highlight)
    {
        foreach (Transform cardTransform in handArea)
        {
            Image image = cardTransform.GetComponent<Image>();

            if (image != null)
            {
                if (highlight)
                {
                    image.color = new Color(1f, 0.7f, 0.7f);
                }
                else
                {
                    image.color = Color.white;
                }
            }
        }
    }
}