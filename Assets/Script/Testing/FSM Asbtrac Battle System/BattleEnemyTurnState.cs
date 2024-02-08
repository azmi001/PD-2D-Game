using System.Collections;
using UnityEngine;

public class BattleEnemyTurnState : BattleBaseState
{
    public override void EnterState(BattleStateManager battle)
    {

    }

    public override void UpdateState(BattleStateManager battle)
    {

    }

    /*IEnumerator EnemyTurn(BattleStateManager battle)
    {
        battle.dialogueText.text = battle.enemyUnit.unitName + " Attacks!";

        yield return new WaitForSeconds(1f);

        bool isDead = battle.playerUnit.TakeDemage(battle.enemyUnit.damage);

        battle.playerHUD.SetHP(battle.playerUnit.currentHP);

        yield return new WaitForSeconds(1f);

        if (isDead)
        {
            battle.SwitchState(battle.lostState);

            yield return new WaitForSeconds(1f);
        } else
        {
            battle.SwitchState(battle.playerTurnState);
        }
    }*/
}
