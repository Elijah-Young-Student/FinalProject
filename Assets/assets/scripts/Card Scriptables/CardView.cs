using UnityEngine;

public class CardView : MonoBehaviour
{
    public Card card;
    public CardManager manager;

    public void Initialize(Card c, CardManager m)
    {
        card = c;
        manager = m;
    }

    public void OnClick()
    {
        manager.SelectCard(this);
    }
}