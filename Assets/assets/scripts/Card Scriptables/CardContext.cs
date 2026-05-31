using System.Collections.Generic;

public class CardContext
{
    public CharacterState owner;
    public CharacterState target;

    public DiceManager diceManager;
    public ControlManager controlManager;

    public Dictionary<string, object> data = new();
}