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

    [SerializeField] private Unit currentUnitPlay;
    
    [SerializeField] private BattleHUD playerHUD;
    [SerializeField] private BattleHUD enemyHUD;

    private int playerIndex;
    private int enemyIndex;

    private bool playerTurnIsDone = false;
    private bool enemyTurnIsDone = true;

    [SerializeField] private BattleState currentState = BattleState.PLAYERTRURN;

    private void Start()
    {
        Init();
    }
    private void OnEnable()
    {
        Funcs.GetAllPlayerUnit += GetAllPlayerUnit;
        Funcs.GetCurrentUnitPlay += GetCurrentUnit;
        Funcs.GetAllEnemyUnit += GetAllEnemy;
        Actions.OnBattleStateChange += ChangeState;
        Actions.OnUnitUsedAction += OnUnitUsedAction;
        Actions.OnShowHoverTarget += OnShowHover;
        Actions.OnUnitDied += RefreshListUnit;
        Actions.CloseListUnit += OnCloseListUnit;
    }

    private void OnDisable()
    {
        Funcs.GetAllPlayerUnit -= GetAllPlayerUnit;
        Funcs.GetCurrentUnitPlay -= GetCurrentUnit;
        Funcs.GetAllEnemyUnit -= GetAllEnemy;
        Actions.OnBattleStateChange -= ChangeState;
        Actions.OnUnitUsedAction -= OnUnitUsedAction;
        Actions.OnShowHoverTarget -= OnShowHover;
        Actions.OnUnitDied -= RefreshListUnit;
        Actions.CloseListUnit -= OnCloseListUnit;
    }

    private List<Unit> GetAllEnemy()
    {
        return enemyUnit;
    }
    private Unit GetCurrentUnit()
    {
        return currentUnitPlay;
    }


    private List<Unit> GetAllPlayerUnit()
    {
        return playerUnit;
    }

    private void Init()
    {
        foreach (var item in playerPrefab)
        {
            GameObject go = Instantiate(item, playerPos);
            go.GetComponent<Unit>().actorType = ACTORTYPE.PLAYER;
            playerUnit.Add(go.GetComponent<Unit>());
        }
        foreach (var item in enemyPrefab)
        {
            GameObject go = Instantiate(item, enemyPos);
            go.GetComponent<Unit>().actorType = ACTORTYPE.ENEMY;
            enemyUnit.Add(go.GetComponent<Unit>());
        }

        currentUnitPlay = playerUnit[playerIndex];
        Actions.AddListenerToGameButton?.Invoke(PlayerAttack, DefenseUp, HealUp,OpenSkill);
    }

    private void OpenSkill()
    {
        Actions.OpenListSkill?.Invoke();
    }

    private void HealUp()
    {
        currentUnitPlay.Heal(20);
        Actions.OnUnitUsedAction?.Invoke(currentUnitPlay);
    }

    private void DefenseUp()
    {
        currentUnitPlay._def *= 3;
        Actions.OnUnitUsedAction?.Invoke(currentUnitPlay);
    }

    private void PlayerAttack()
    {
        Actions.OpenListUnit?.Invoke(enemyUnit);
        Actions.OnTargetedUnit += OnTargetedUnit;
    }

    private void OnCloseListUnit()
    {
        if (Actions.OnTargetedUnit == null)
            return;
        Delegate[] subs = Actions.OnTargetedUnit.GetInvocationList();
        foreach (var item in subs)
        {
            Actions.OnTargetedUnit -= (Action<Unit>)item;
        }
    }
    private void RefreshListUnit(Unit targetunit)
    {
        switch (targetunit.actorType)
        {
            case ACTORTYPE.PLAYER:
                playerUnit.Remove(targetunit);
                Destroy(targetunit.gameObject);
                break;
            case ACTORTYPE.ENEMY:
                enemyUnit.Remove(targetunit);
                Destroy(targetunit.gameObject);
                break;
        }
    }
    private void OnShowHover(bool isShowing, int index, ACTORTYPE actor)
    {
        Debug.Log(actor);
        if (isShowing)
        {
            switch (actor)
            {
                case ACTORTYPE.PLAYER:
                    playerPos.GetChild(index).GetComponentInChildren<SpriteRenderer>().color = Color.green;
                    break;
                case ACTORTYPE.ENEMY:
                    enemyPos.GetChild(index).GetComponentInChildren<SpriteRenderer>().color = Color.green;
                    break;
                default:
                    break;
            }
        }
        else
        {
            switch (actor)
            {
                case ACTORTYPE.PLAYER:
                    playerPos.GetChild(index).GetComponentInChildren<SpriteRenderer>().color = Color.white;
                    break;
                case ACTORTYPE.ENEMY:
                    enemyPos.GetChild(index).GetComponentInChildren<SpriteRenderer>().color = Color.white;
                    break;
                default:
                    break;
            }

        }
    }
    private void OnUnitUsedAction(Unit targetunit)
    {
        switch (targetunit.actorType)
        {
            case ACTORTYPE.PLAYER:
                playerIndex++;
                break;
            case ACTORTYPE.ENEMY:
                enemyIndex++;
                break;
            default:
                break;
        }
        ChangeState(BattleState.CHECK);
    }

    private void OnTargetedUnit(Unit target)
    {
        target.TakeDemage(currentUnitPlay.character.damage, target._def, currentUnitPlay.character.thisUnitElement);
        Actions.OnUnitUsedAction?.Invoke(Funcs.GetCurrentUnitPlay());
        Actions.OnTargetedUnit -= OnTargetedUnit;
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
                Actions.AddListenerToGameButton?.Invoke(PlayerAttack, DefenseUp, HealUp, OpenSkill);
                currentUnitPlay = playerUnit[playerIndex];
                break;
            case BattleState.ENEMYTURN:
                currentUnitPlay = enemyUnit[enemyIndex];
                Actions.IsDisableAllButton?.Invoke(true);
                StartCoroutine(EnemyAttack());
                break;
            case BattleState.WON:
                Debug.Log("Won");
                break;
            case BattleState.LOST:
                Debug.Log("Lost");
                break;
            case BattleState.CHECK:
                if (enemyUnit == null || enemyUnit.Count <= 0)
                {
                    ChangeState(BattleState.WON);
                    return;
                }
                if(playerUnit == null || playerUnit.Count <= 0)
                {
                    ChangeState(BattleState.LOST);
                    return;
                }
                if (playerTurnIsDone && !enemyTurnIsDone)
                {
                    if (enemyIndex >= enemyUnit.Count)
                    {
                        playerTurnIsDone = false;
                        enemyTurnIsDone = true;
                        enemyIndex = 0;
                        ChangeState(BattleState.CHECK);
                    }
                    else
                    {
                        ChangeState(BattleState.ENEMYTURN);
                    }
                }
                if(!playerTurnIsDone && enemyTurnIsDone)
                {
                    if (playerIndex >= playerUnit.Count)
                    {
                        playerTurnIsDone = true;
                        enemyTurnIsDone = false;
                        playerIndex = 0;
                        ChangeState(BattleState.CHECK);
                    }
                    else
                    {
                        ChangeState(BattleState.PLAYERTRURN);
                    }
                    break;
                }
                break;
            default:
                break;
        }
    }
    private IEnumerator EnemyAttack()
    {
        int rand = UnityEngine.Random.Range(0, playerUnit.Count);
        Debug.Log("Enemy attack player" + rand);
        playerUnit[rand].TakeDemage(enemyUnit[enemyIndex].character.damage, playerUnit[rand]._def, enemyUnit[enemyIndex].character.thisUnitElement);
        yield return new WaitForSeconds(1f);
        Actions.OnUnitUsedAction?.Invoke(currentUnitPlay);
    }
}
