//using UnityEngine;

//[CreateAssetMenu(menuName = "Cards/Effects/Apply Status")]
//public class ApplyStatusEffect : CardEffect
//{
//    public StatusEffect status;

//    public int amount;

//    public bool forceEnemy;

//    public override IEnumerator Resolve(
//        CardManager manager,
//        CardContext context
//    )
//    {
//        PlayerController target;

//        if (forceEnemy)
//        {
//            target = manager.enemyPlayer;
//        }
//        else
//        {
//            target = context.targetPlayer;
//        }

//        target.ApplyStatus(status, amount);

//        yield break;
//    }
//}