using UnityEngine;
using UnityEngine.UI;

public class CardDisplay : MonoBehaviour
{
    private Card cardData;
    private CardManager handUI;

    public void Setup(GameObject card, CardManager ui)
    {
        cardData = card.GetComponent<Card>();
        handUI = ui;

        Button button = GetComponent<Button>();

        if (button != null)
        {
            button.onClick.AddListener(OnClicked);
        }
    }

    void OnClicked()
    {
        handUI.SelectCard(gameObject);
    }
}