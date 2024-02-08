using System.Collections;
using UnityEngine;

public class BattlePlayerTurnState : BattleBaseState
{
    public override void EnterState(BattleStateManager battle)
    {

    }

    public override void UpdateState(BattleStateManager battle)
    {

    }

    /*IEnumerator PlayerAttack(BattleStateManager battle)
    {
        //Memberiu musuh serangan
        bool isDead = battle.enemyUnit.TakeDemage(battle.playerUnit.damage);

        battle.enemyHUD.SetHP(battle.enemyUnit.currentHP);
        battle.dialogueText.text = "The attack is successful!";

        yield return new WaitForSeconds(2f);

        //Mengecek apakah musuh mati atau tidak
        if (isDead)
        {
            //Menyelesaikan battle
            battle.SwitchState(battle.wonState);

            yield return new WaitForSeconds(1f);
        } else
        {
            //memasuki musuh menyerang
            battle.SwitchState(battle.enemyTurnState);
        }
    }*/
}
