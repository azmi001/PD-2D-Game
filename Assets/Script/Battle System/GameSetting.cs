using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Progress;

public partial class GameSetting : MonoBehaviour
{
    [SerializeField] private GameObject[] playerPrefab;
    //[SerializeField] private GameObject[] enemyPrefab;

    [SerializeField] private Transform playerPos;
    [SerializeField] private Transform enemyPos;

    [SerializeField] private List<Unit> playerUnit = new();
    [SerializeField] private List<Unit> enemyUnit = new();

    [SerializeField] private Unit currentUnitPlay;

    [SerializeField] private Text DialogText;



    private int playerIndex;
    private int enemyIndex;

    private bool playerTurnIsDone = false;
    private bool enemyTurnIsDone = true;

    [SerializeField] private BattleState currentState = BattleState.PLAYERTRURN;

    private void Start()
    {
        StartCoroutine(Init());
    }
    private void OnEnable()
    {
        //get semua unit player
        Funcs.GetAllPlayerUnit += GetAllPlayerUnit;
        //get unit yang bermain skarang
        Funcs.GetCurrentUnitPlay += GetCurrentUnit;
        //get semua unit enemy
        Funcs.GetAllEnemyUnit += GetAllEnemy;
        //subscribe ketika battlestate berubah
        Actions.OnBattleStateChange += ChangeState;
        //subscribe ketika unit sudah melakukan action
        Actions.OnUnitUsedAction += OnUnitUsedAction;
        //subscribe ketika mouse berada di tombol unit a/b/c (bukan di klik)
        Actions.OnShowHoverTarget += OnShowHover;
        //subscribe ketika ada unit yang mati maka refresh semua list
        Actions.OnUnitDied += RefreshListUnit;
        //subscribe ketika panel unit sudah ditutup
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
    //setup game
    IEnumerator Init()
    {
        for (int i = 0; i < playerPrefab.Length; i++)
        {
            GameObject go = Instantiate(playerPrefab[i], playerPos.GetChild(i));
            go.transform.GetChild(0).GetComponent<SpriteRenderer>().sortingOrder = i;
            go.transform.GetChild(1).GetComponent<Canvas>().sortingOrder = i;
            go.GetComponent<Unit>().actorType = ACTORTYPE.PLAYER;
            playerUnit.Add(go.GetComponent<Unit>());
        }

        //mengambil data enemy dari scribtabke game object story quest
        StoryQuest quest = FindObjectOfType<GameManager>().currentQuest;

        for (int i = 0;i < quest.EnemyPrefabs.Length; i++)
        {
            GameObject go = Instantiate(quest.EnemyPrefabs[i], enemyPos.GetChild(i));
            go.transform.GetChild(0).GetComponent<SpriteRenderer>().sortingOrder = i;
            go.transform.GetChild(1).GetComponent<Canvas>().sortingOrder = i;
            go.GetComponent<Unit>().actorType = ACTORTYPE.ENEMY;
            go.transform.GetChild(0).GetComponent<SpriteRenderer>().flipX = true;
            enemyUnit.Add(go.GetComponent<Unit>());
        }

        currentUnitPlay = playerUnit[playerIndex];
        Actions.AddListenerToGameButton?.Invoke(PlayerAttack, DefenseUp, HealUp,OpenSkill);
        DialogText.text = "The Battle Begin";
        yield return new WaitForSeconds(1f);
        currentUnitPlay.transform.GetChild(0).GetComponentInChildren<SpriteRenderer>().color = Color.yellow;
        DialogText.text = "Player Turn ! " + currentUnitPlay.character.unitName;
    }
    #region funcs
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
    #endregion
    #region actions
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
                Destroy(targetunit.transform.parent.gameObject);
                break;
            case ACTORTYPE.ENEMY:
                enemyUnit.Remove(targetunit);
                Destroy(targetunit.transform.parent.gameObject);
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
                    playerPos.GetChild(index).GetChild(0).GetComponentInChildren<SpriteRenderer>().color = Color.gray;
                    break;
                case ACTORTYPE.ENEMY:
                    enemyPos.GetChild(index).GetChild(0).GetComponentInChildren<SpriteRenderer>().color = Color.gray;
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
                    playerPos.GetChild(index).GetChild(0).GetComponentInChildren<SpriteRenderer>().color = Color.white;
                    break;
                case ACTORTYPE.ENEMY:
                    enemyPos.GetChild(index).GetChild(0).GetComponentInChildren<SpriteRenderer>().color = Color.white;
                    break;
                default:
                    break;
            }

        }
    }
    private void OnUnitUsedAction(Unit targetunit)
    {
        targetunit.transform.GetChild(0).GetComponentInChildren<SpriteRenderer>().color = Color.white;
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
                currentUnitPlay.transform.GetChild(0).GetComponentInChildren<SpriteRenderer>().color = Color.yellow;
                DialogText.text = "Player Turn ! " + currentUnitPlay.character.unitName;
                break;
            case BattleState.ENEMYTURN:
                currentUnitPlay = enemyUnit[enemyIndex];
                Actions.IsDisableAllButton?.Invoke(true);
                DialogText.text = "Enemy Turn ! " + currentUnitPlay.character.unitName;
                currentUnitPlay.transform.GetChild(0).GetComponentInChildren<SpriteRenderer>().color = Color.yellow;
                StartCoroutine(EnemyAttack());
                break;
            case BattleState.WON:
                Debug.Log("Won");
                DialogText.text = "Player Win The Battle!";
                break;
            case BattleState.LOST:
                Debug.Log("Lost");
                DialogText.text = "Enemy Win The Battle!";
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
    #endregion
    //auto attack enemy
    private IEnumerator EnemyAttack()
    {
        int rand = UnityEngine.Random.Range(0, playerUnit.Count);
        Debug.Log("Enemy attack player" + rand);
        playerUnit[rand].TakeDemage(enemyUnit[enemyIndex].character.damage, playerUnit[rand]._def, enemyUnit[enemyIndex].character.thisUnitElement);
        yield return new WaitForSeconds(1f);
        Actions.OnUnitUsedAction?.Invoke(currentUnitPlay);
    }
}
