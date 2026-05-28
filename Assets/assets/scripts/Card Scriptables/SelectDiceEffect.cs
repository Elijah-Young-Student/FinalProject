//using UnityEngine;

//[CreateAssetMenu(menuName = "Cards/Effects/Select Dice")]
//public class SelectDiceEffect : CardEffect
//{
//    public int amountToSelect;

//    public override IEnumerator Resolve(
//        CardManager manager,
//        CardContext context
//    )
//    {
//        manager.StartDiceSelection(amountToSelect);

//        // WAIT until player finishes selecting
//        yield return new WaitUntil(() =>
//            manager.SelectedDice.Count >= amountToSelect
//        );

//        context.selectedDice = manager.SelectedDice;

//        manager.EndDiceSelection();
//    }
//}