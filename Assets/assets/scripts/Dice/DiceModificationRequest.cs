using UnityEngine;

public class DiceModificationRequest
{
    public bool awaitingSelection = false;

    public int diceToModify = 1;

    public bool allowAnyFace = true;

    public string[] allowedFaces;
}