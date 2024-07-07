using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
//using static UnityEditor.Progress;

public partial class GameSetting : MonoBehaviour
{
    //[SerializeField] private GameObject[] playerPrefab;
    //[SerializeField] private GameObject[] enemyPrefab;

    [SerializeField] private Transform playerPos;
    [SerializeField] private Transform enemyPos;

    [SerializeField] private List<Unit> playerUnit = new();
    [SerializeField] private List<Unit> enemyUnit = new();

    [SerializeField] private Unit currentUnitPlay;

    [SerializeField] private Text DialogText;

    [SerializeField] private SpriteRenderer bg;

    private int playerIndex;
    private int enemyIndex;

    private bool playerTurnIsDone = false;
    private bool enemyTurnIsDone = true;

    private bool winning = false;
    private bool loosing = false;

    [SerializeField] private BattleState currentState = BattleState.PLAYERTRURN;

    private void Start()
    {
        winning = false;
        loosing = false;
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
        //Unit _hero = Funcs.GetDatabaseUnit?.Invoke().GetUnit(Funcs.GetAkun().teamHeroes[1]);
        if(Funcs.GetCurrentQuest().bg!=null)
            bg.sprite = Funcs.GetCurrentQuest().bg;
        for (int i = 0; i < Funcs.GetAkun().teamHeroes.Count; i++)
        {
            try
            {
                Unit hero = Funcs.GetDatabaseUnit?.Invoke().GetUnit(Funcs.GetAkun().teamHeroes[i]);
                GameObject go = Instantiate(hero.gameObject, playerPos.GetChild(i));
                go.transform.GetChild(0).GetComponent<SpriteRenderer>().sortingOrder = i;
                go.transform.GetChild(1).GetComponent<Canvas>().sortingOrder = i;
                go.GetComponent<Unit>().actorType = ACTORTYPE.PLAYER;
                playerUnit.Add(go.GetComponent<Unit>());

            }
            catch
            {
                continue;
            }
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
        DialogText.text = "Player Turn ! " + currentUnitPlay.character.charaData.unitName;
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
        currentUnitPlay.Heal(100);//jumlah Hp Heal Player
        Actions.OnUnitUsedAction?.Invoke(currentUnitPlay);
    }

    private void DefenseUp()
    {
        currentUnitPlay.DefUp(3);//Jumlah def *X dikali brp
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
    private async void RefreshListUnit(Unit targetunit)
    {
        Animator animator = targetunit.GetComponentInChildren<Animator>();
        animator.Play("KO");
        while (!animator.GetCurrentAnimatorStateInfo(0).IsName("KO"))
        {
            await Task.Yield();
        }
        float animLength = animator.GetCurrentAnimatorStateInfo(0).length;
        await Task.Delay((int)(animLength * 1000));

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
        //fix klo enemy nya 1 gak langsung win player jadi harus nunggu semua player kelar turn baru win klo gak di fix
        ChangeState(BattleState.CHECK);
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
    private async void OnUnitUsedAction(Unit targetunit)
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
        targetunit.LastTurn++;
        await Task.Delay(1000);
        ChangeState(BattleState.CHECK);
    }

    private void OnTargetedUnit(Unit target)
    {
        currentUnitPlay.GetComponentInChildren<Animator>().Play("Attack");
        target.TakeDemage(currentUnitPlay.character.charaData.damage, target._def, currentUnitPlay.character.thisUnitElement);
        Actions.OnUnitUsedAction?.Invoke(Funcs.GetCurrentUnitPlay());
        Actions.OnTargetedUnit -= OnTargetedUnit;
    }

    public async void ChangeState(BattleState newState)
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
                currentUnitPlay.CurrentTurn++;
                if (currentUnitPlay.CurrentTurn> currentUnitPlay.LastTurn)
                {
                    if(currentUnitPlay.isdefup == true)
                    {
                        currentUnitPlay.DefDefault();
                    }
                }
                if (currentUnitPlay == null) return;
                currentUnitPlay.transform.GetChild(0).GetComponentInChildren<SpriteRenderer>().color = Color.yellow;
                DialogText.text = "Player Turn ! " + currentUnitPlay.character.charaData.unitName;
                break;
            case BattleState.ENEMYTURN:
                currentUnitPlay = enemyUnit[enemyIndex];
                Actions.IsDisableAllButton?.Invoke(true);
                DialogText.text = "Enemy Turn ! " + currentUnitPlay.character.charaData.unitName;
                currentUnitPlay.transform.GetChild(0).GetComponentInChildren<SpriteRenderer>().color = Color.yellow;
                currentUnitPlay.CurrentTurn++;
                if (currentUnitPlay.CurrentTurn > currentUnitPlay.LastTurn)
                {
                    if (currentUnitPlay.isdefup == true)
                    {
                        currentUnitPlay.DefDefault();
                    }
                }
                StartCoroutine(EnemyAttack());
                break;
            case BattleState.WON:
                if (winning) return;
                winning = true;
                Debug.Log("Won");
                DialogText.text = "Player Win The Battle!";
                Actions.onQuestFinis?.Invoke(FindObjectOfType<GameManager>().currentQuest);
                await Task.Delay(2000);
                Actions.OnResultBattle?.Invoke(true);
                //SceneManager.LoadScene("Hub");
                break;
            case BattleState.LOST:
                if (loosing) return;
                loosing = true;
                Debug.Log("Lost");
                DialogText.text = "Enemy Win The Battle!";
                await Task.Delay(2000);
                Actions.OnResultBattle?.Invoke(false);
                //SceneManager.LoadScene("Hub");
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
        //int rand = UnityEngine.Random.Range(0, playerUnit.Count);
        //Debug.Log("Enemy attack player" + rand);
        //playerUnit[rand].TakeDemage(enemyUnit[enemyIndex].character.damage, playerUnit[rand]._def, enemyUnit[enemyIndex].character.thisUnitElement);
        currentUnitPlay.UnitAction();
        yield return new WaitForSeconds(1f);
        Actions.OnUnitUsedAction?.Invoke(currentUnitPlay);
    }
}
