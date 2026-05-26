using UnityEngine;
using UnityEngine.UI;

public class CardDisplay : MonoBehaviour
{
    private Card cardData;
    private CardManager handUI;

    public void Setup(Card card, CardManager ui)
    {
        cardData = card;
        handUI = ui;

        Button button = GetComponent<Button>();

        if (button != null)
        {
            button.onClick.AddListener(OnClicked);
        }
    }

    void OnClicked()
    {
        handUI.SelectCard(gameObject, cardData);
    }
}