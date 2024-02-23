using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public enum BattleState { START, PLAYERTRURN, ENEMYTURN, WON, LOST }

public class BattleSystem : MonoBehaviour
{
	public static BattleSystem instance { get; private set; }

	public SkillController sc;

	[Header("Game Object")]
	public GameObject playerPrefabs;
	public GameObject enemyPrefabs;

	public Transform playerBattleStation;
	public Transform enemyBattleStation;

	Unit _playerUnit;
	Unit _enemyUnit;

    [Header("Total Damage Element %")]
    public int elDmg = 15;
    [Header("Total Varian Demage %")]
    public int varDmg = 20;

	[Header("User Interface")]
	public Text dialogueText;
    public Button attackButton;
    public Button healButton;
	public Button defenseButton;
    public BattleHUD playerHUD;
	public BattleHUD enemyHUD;
	
	public BattleState state;
	
	public string menang;
	public string kalah;

	public bool isTurn = false;
	public bool isPlayerDefense = false;

    private void Awake()
    {
        if (instance == null)
		{
			instance = this;
		} else
		{
			Destroy(gameObject);
			return;
		}
    }

    // Start is called before the first frame update
    void Start()
	{
		state = BattleState.START;
		StartCoroutine(SetupBattle());
	}

	IEnumerator SetupBattle()
	{
		GameObject playerGO = Instantiate(playerPrefabs, playerBattleStation);
		_playerUnit = playerGO.GetComponent<Unit>();

		sc.DisplaySkill(_playerUnit);
		GameObject enemyGO= Instantiate(enemyPrefabs, enemyBattleStation);
		_enemyUnit = enemyGO.GetComponent<Unit>();

		dialogueText.text = "A wild " + _enemyUnit.character.unitName + " approaches...";

		playerHUD.SetHUD(_playerUnit);
		enemyHUD.SetHUD(_enemyUnit);

		yield return new WaitForSeconds(2f);

		state = BattleState.PLAYERTRURN;
		PlayerTurn();
	}

	IEnumerator PlayerAttack()
	{
        DisableInteraction();

        // Damage the enemy
        //bool isDead = enemyUnit.TakeDemage(playerUnit.damage, enemyUnit.deffense);

        bool isDead = _enemyUnit.TakeDemage(
			_playerUnit.character.damage, 
			_enemyUnit.character.deffense, 
			_playerUnit.character.thisUnitElement);

        enemyHUD.SetHP(_enemyUnit.currentHP);
		dialogueText.text = "The attack is successul!";

		yield return new WaitForSeconds(2f);

		//Check f the enemy is dead
		if(isDead)
		{
			// End the battle
			state = BattleState.WON;
			EndBattle();
			_playerUnit.GainExp(100);
			playerHUD.SetHUD(_playerUnit);

			yield return new WaitForSeconds(1f);
			//SceneManager.LoadScene(menang);
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
		dialogueText.text = _enemyUnit.character.unitName + " attacks!";
		
		yield return new WaitForSeconds(1f);

		if(isPlayerDefense == true)
		{
            //bool isDead = playerUnit.TakeDemage(enemyUnit.damage, playerUnit.deffense * 3);
            bool isDead = _playerUnit.TakeDemage(
				_enemyUnit.character.damage, 
				_playerUnit.character.deffense * 3, 
				_enemyUnit.character.thisUnitElement);

            if (isDead)
            {
                state = BattleState.LOST;
                EndBattle();

                yield return new WaitForSeconds(1f);
                SceneManager.LoadScene(kalah);
            }
            else
            {
                state = BattleState.PLAYERTRURN;
                EnableInteraction();
                PlayerTurn();
                isTurn = false;
				isPlayerDefense = false;
            }
        }
		else
		{
            //bool isDead = playerUnit.TakeDemage(enemyUnit.damage, playerUnit.deffense);
            bool isDead = _playerUnit.TakeDemage(
				_enemyUnit.character.damage, 
				_playerUnit.character.deffense, 
				_enemyUnit.character.thisUnitElement);

            if (isDead)
            {
                state = BattleState.LOST;
                EndBattle();

                yield return new WaitForSeconds(1f);
                SceneManager.LoadScene(kalah);
            }
            else
            {
                state = BattleState.PLAYERTRURN;
                EnableInteraction();
                PlayerTurn();
                isTurn = false;
            }
        }

		playerHUD.SetHP(_playerUnit.currentHP);

	}

	void EndBattle()
	{
		if(state == BattleState.WON)
		{
			dialogueText.text = "You won the battle!";
		} else if (state == BattleState.LOST)
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
		defenseButton.interactable = true;
    }

    void DisableInteraction()
    {
        attackButton.interactable = false;
        healButton.interactable = false;
		defenseButton.interactable = false;
    }

    IEnumerator PlayerHeal()
	{
        DisableInteraction();

        _playerUnit.Heal(100);

		playerHUD.SetHP(_playerUnit.currentHP);
		dialogueText.text = "You feel renewed strength!";

		yield return new WaitForSeconds(2f);

		state = BattleState.ENEMYTURN;
		StartCoroutine(EnemyTurn());
	}

	IEnumerator PlayerDefense()
	{
        DisableInteraction();

        isPlayerDefense = true;

        dialogueText.text = "Player try to Defense!";

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
		
        if (isTurn != true)
        {
            StartCoroutine(PlayerHeal());

            isTurn = true;
        }
    }

    public void OnDefeseButton()
    {
        if (state != BattleState.PLAYERTRURN)
            return;

        if (isTurn != true)
        {
            StartCoroutine(PlayerDefense());

            isTurn = true;
        }
    }
}
