using System.Collections;
using UnityEngine;

public abstract class CardEffect : ScriptableObject
{
    public abstract IEnumerator Execute(CardContext context);
}