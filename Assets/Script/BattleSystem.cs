using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public enum BattleState { START, PLAYERTRURN, ENEMYTURN, WON, LOST }

public class BattleSystem : MonoBehaviour
{
	[Header("Game Object")]
	public GameObject playerPrefabs;
	public GameObject enemyPrefabs;

	public Transform playerBattleStation;
	public Transform enemyBattleStation;

	Unit playerUnit;
	Unit enemyUnit;

	[Header("User Interface")]
	public Text dialogueText;
    public Button attackButton;
    public Button healButton;
    public BattleHUD playerHUD;
	public BattleHUD enemyHUD;
	
	public BattleState state;
	
	public string menang;
	public string kalah;

	public bool isTurn = false;

	// Start is called before the first frame update
	void Start()
	{
		state = BattleState.START;
		StartCoroutine(SetupBattle());
	}

	IEnumerator SetupBattle()
	{
		GameObject playerGO = Instantiate(playerPrefabs, playerBattleStation);
		playerUnit = playerGO.GetComponent<Unit>();

		GameObject enemyGO= Instantiate(enemyPrefabs, enemyBattleStation);
		enemyUnit = enemyGO.GetComponent<Unit>();

		dialogueText.text = "A wild " + enemyUnit.unitName + " approaches...";

		playerHUD.SetHUD(playerUnit);
		enemyHUD.SetHUD(enemyUnit);

		yield return new WaitForSeconds(2f);

		state = BattleState.PLAYERTRURN;
		PlayerTurn();
	}

	IEnumerator PlayerAttack()
	{
        DisableInteraction();

        // Damage the enemy
        bool isDead = enemyUnit.TakeDemage(playerUnit.damage, enemyUnit.deffense);

		enemyHUD.SetHP(enemyUnit.currentHP);
		dialogueText.text = "The attack is successul!";

		yield return new WaitForSeconds(2f);

		//Check f the enemy is dead
		if(isDead)
		{
			// End the battle
			state = BattleState.WON;
			EndBattle();

			yield return new WaitForSeconds(1f);
			SceneManager.LoadScene(menang);
		} else
		{
			// Enemy turn
			state = BattleState.ENEMYTURN;
			StartCoroutine(EnemyTurn());
		}
		//Change state based on what happened
	}

	IEnumerator EnemyTurn()
	{
		dialogueText.text = enemyUnit.unitName + " attacks!";
		
		yield return new WaitForSeconds(1f);

		bool isDead = playerUnit.TakeDemage(enemyUnit.damage, playerUnit.deffense);

		playerHUD.SetHP(playerUnit.currentHP);

		yield return new WaitForSeconds(1f);

		if(isDead)
		{
			state = BattleState.LOST;
			EndBattle();
			
			yield return new WaitForSeconds(1f);
			SceneManager.LoadScene(kalah);
		} else
		{
			state = BattleState.PLAYERTRURN;
            EnableInteraction();
            PlayerTurn();
			isTurn = false;
		}
	}

	void EndBattle()
	{
		if(state == BattleState.WON)
		{
			dialogueText.text = "You won the battle!";
		} else if ( state == BattleState.LOST)
		{
			dialogueText.text = "You were defeated.";
		}
	}

	void PlayerTurn()
	{
		dialogueText.text = "Choose an action:";
	}

    void EnableInteraction()
    {
        attackButton.interactable = true;
        healButton.interactable = true;
    }

    void DisableInteraction()
    {
        attackButton.interactable = false;
        healButton.interactable = false;
    }

    IEnumerator PlayerHeal()
	{
		playerUnit.Heal(15);

		playerHUD.SetHP(playerUnit.currentHP);
		dialogueText.text = "You feel renewed strength!";

		yield return new WaitForSeconds(2f);

		state = BattleState.ENEMYTURN;
		StartCoroutine(EnemyTurn());
	}

	public void OnAttackButton()
	{
		if (state != BattleState.PLAYERTRURN)
			return;

		if (isTurn != true)
		{
			StartCoroutine(PlayerAttack());

			isTurn = true;
		}
		
	}

	public void OnHealButton()
	{
		if (state != BattleState.PLAYERTRURN)
			return;

		StartCoroutine(PlayerHeal());
	}
}
