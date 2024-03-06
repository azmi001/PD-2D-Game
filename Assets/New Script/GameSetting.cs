using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSetting : MonoBehaviour
{
    [SerializeField] private GameObject[] playerPrefab;
    [SerializeField] private GameObject[] enemyPrefab;

    [SerializeField] private Transform playerPos;
    [SerializeField] private Transform enemyPos;

    [SerializeField]private List<Unit> playerUnit = new();
    [SerializeField] private List<Unit> enemyUnit = new();

    [SerializeField] private BattleHUD playerHUD;
    [SerializeField] private BattleHUD enemyHUD;

    private int playerIndex;
    private int enemyIndex;

    public bool playerTurnIsDone = false;
    public bool enemyTurnIsDone = false;

    [SerializeField] private BattleState currentState = BattleState.PLAYERTRURN;

    private void Start()
    {
        Init();
    }
    private void OnEnable()
    {
        Actions.AttackToEnemy += OnAttackToEnemy;
    }
    private void OnDisable()
    {
        Actions.AttackToEnemy -= OnAttackToEnemy;
    }
    private void OnAttackToEnemy(int value)
    {
        bool isUnitDead = enemyUnit[value].TakeDemage(playerUnit[playerIndex].character.damage, enemyUnit[value]._def, playerUnit[playerIndex].character.thisUnitElement);
        if (isUnitDead)
        {
            GameObject go = enemyUnit[value].gameObject;
            enemyUnit.Remove(enemyUnit[value]);
            Destroy(go);

        }
        CheckPlayerTurn();
    }

    private void Init()
    {
        foreach (var item in playerPrefab)
        {
            GameObject go = Instantiate(item, playerPos);
            playerUnit.Add(go.GetComponent<Unit>());
        }
        foreach (var item in enemyPrefab)
        {
            GameObject go = Instantiate(item, enemyPos);
            enemyUnit.Add(go.GetComponent<Unit>());
        }

        Actions.AddListenerToGameButton?.Invoke(AttackToEnemy, DefenseUp, HealUp);
    }

    private void HealUp()
    {
        playerUnit[playerIndex].Heal(playerUnit[playerIndex].character.Heal);
        CheckPlayerTurn();
    }

    private void DefenseUp()
    {
        playerUnit[playerIndex]._def *= 3;
        CheckPlayerTurn();
    }

    private void AttackToEnemy()
    {
        Actions.OpenListEnemy?.Invoke(enemyUnit.Count);
    }

    private void CheckPlayerTurn()
    {
        playerIndex++;
        if (playerIndex >= playerUnit.Count)
        {
            playerTurnIsDone = true;
            ChangeState(BattleState.CHECK);
        }
        else
        {
            Actions.AddListenerToGameButton?.Invoke(AttackToEnemy, DefenseUp, HealUp);
            ChangeState(BattleState.PLAYERTRURN);
        }
    }

    public void ChangeState(BattleState newState)
    {
        currentState = newState;
        switch (currentState)
        {
            case BattleState.START:
                break;
            case BattleState.PLAYERTRURN:
                Actions.IsDisableAllButton?.Invoke(false);
                break;
            case BattleState.ENEMYTURN:
                StartCoroutine(EnemyAttack());
                Actions.IsDisableAllButton?.Invoke(true);
                break;
            case BattleState.WON:
                Debug.Log("Won");
                break;
            case BattleState.LOST:
                Debug.Log("Lost");
                break;
            case BattleState.CHECK:
                if (playerTurnIsDone)
                {
                    if (enemyUnit != null||enemyUnit.Count>0)
                    {
                        playerIndex = 0;
                        playerTurnIsDone = false;
                        ChangeState(BattleState.ENEMYTURN);
                    }
                    if(enemyUnit == null || enemyUnit.Count<=0)
                    {
                        playerIndex = 0;
                        playerTurnIsDone = false;
                        ChangeState(BattleState.WON);
                    }
                }
                if (enemyTurnIsDone)
                {
                    if (playerUnit != null || playerUnit.Count > 0)
                    {
                        enemyIndex = 0;
                        enemyTurnIsDone = false;
                        ChangeState(BattleState.PLAYERTRURN);
                    }
                    if (playerUnit == null || playerUnit.Count <= 0)
                    {
                        enemyIndex = 0;
                        enemyTurnIsDone = false;
                        ChangeState(BattleState.LOST);
                    }

                }
                break;
            default:
                break;
        }
    }

    private IEnumerator EnemyAttack()
    {
        while (enemyIndex<enemyUnit.Count)
        {
            int rand = UnityEngine.Random.Range(0, playerUnit.Count);
            Debug.Log("Enemy attack player" + rand);
            bool IsUnitDead = playerUnit[rand].TakeDemage(enemyUnit[enemyIndex].character.damage, playerUnit[rand]._def, enemyUnit[enemyIndex].character.thisUnitElement);
            if (IsUnitDead)
            {
                GameObject go = playerUnit[rand].gameObject;
                playerUnit.Remove(playerUnit[rand]);
                Destroy(go);
            }
            
            enemyIndex++;
            yield return new WaitForSeconds(1f);
        }
        enemyTurnIsDone = true;
        ChangeState(BattleState.CHECK);
    }
}
