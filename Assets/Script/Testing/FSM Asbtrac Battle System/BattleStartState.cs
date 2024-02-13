using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using System;

public class BattleStartState : BattleBaseState
{
    public override void EnterState(BattleStateManager battle)
    {
        Debug.Log("Mulai Battle!");
        //battle.playerBattleStation = null;
        //StartCoroutine(SetupBattle(battle));
    }

    public override void UpdateState(BattleStateManager battle)
    {
        Debug.Log("Battle permainan sedang berlangsung!");
    }

    /*IEnumerator SetupBattle(BattleStateManager battle)
    {
        //untuk menggunakan fungsi instantiate mau tiak mau di base state menambahkan monobehavior
        GameObject playerGO = Instantiate(battle.playerPrefabs, battle.playerBattleStation);
        battle.playerUnit = playerGO.GetComponent<Unit>();

        GameObject enemyGO = Instantiate(battle.enemyPrefabs, battle.enemyBattleStation);
        battle.enemyUnit = enemyGO.GetComponent<Unit>();

        battle.dialogueText.text = "A wild " + battle.enemyUnit.name + " approaches....";

        battle.playerHUD.SetHUD(battle.playerUnit);
        battle.enemyHUD.SetHUD(battle.enemyUnit);

        yield return new WaitForSeconds(2f);

        battle.SwitchState(battle.playerTurnState);
    }*/
}
