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
        DialogText.text = "Player Turn ! " + currentUnitPlay._character.charaData.unitName;
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
        StartCoroutine(OnTargetedUnitCoroutine(target));
    }

    IEnumerator OnTargetedUnitCoroutine(Unit target)
    {
        currentUnitPlay.GetComponentInChildren<Animator>().Play("Attack");
        currentUnitPlay.transform.position = target.opponentPos.position;
        Actions.IsDisableAllButton?.Invoke(true);
        target.TakeDemage(currentUnitPlay._character.charaData.damage, target._def, currentUnitPlay._character.thisUnitElement);
        float animLength = currentUnitPlay.GetComponentInChildren<Animator>().GetCurrentAnimatorStateInfo(0).length;
        yield return new WaitForSeconds(animLength);
        currentUnitPlay.transform.localPosition = Vector3.zero;
        Actions.OnUnitUsedAction?.Invoke(Funcs.GetCurrentUnitPlay());
        Actions.IsDisableAllButton?.Invoke(false);
        Actions.OnTargetedUnit -= OnTargetedUnit;
    }
    private async Task WaitForAnimation(Animator anim)
    {
        // Mendapatkan panjang animasi dari clip yang sedang dimainkan
        float animationLength = anim.GetCurrentAnimatorStateInfo(0).length;

        // Tunggu sesuai panjang animasi
        await Task.Delay((int)(animationLength * 1000));
    }

    public async void ChangeState(BattleState newState)
    {
        currentState = newState;
        switch (currentState)
        {
            case BattleState.START:
                break;
            case BattleState.PLAYERTRURN:
                //mengaktifkan seluruh button yang ada pada game
                //
                Actions.IsDisableAllButton?.Invoke(false);
                // memberikan fungsi pada setiap button action seperti attack,heal,dll
                //
                Actions.AddListenerToGameButton?.Invoke(PlayerAttack, DefenseUp, HealUp, OpenSkill);
                //aktor yang bermain sekarang berganti ke karakter player sesuai urutan index.
                //
                currentUnitPlay = playerUnit[playerIndex];
                //menambahkan turn pada unit yang berjalan sekarang
                //
                currentUnitPlay.CurrentTurn++;
                // pemberian if statement jika turn sekarang lebih tinggi dari last turn dan jika karakter sedang defense up, maka defense dikembalikan ke normal
                //
                if (currentUnitPlay.CurrentTurn> currentUnitPlay.LastTurn) 
                {
                    if(currentUnitPlay.isdefup == true)
                    {
                        currentUnitPlay.DefDefault();
                    }
                }
                if (currentUnitPlay == null) return;
                //karakter yang berjalan akan mempunyai highlight kuning
                //
                currentUnitPlay.transform.GetChild(0).GetComponentInChildren<SpriteRenderer>().color = Color.yellow;
                //mengganti dialog teks yang sesuai dibutuhkan
                //
                DialogText.text = "Player Turn ! " + currentUnitPlay._character.charaData.unitName;
                break;
            case BattleState.ENEMYTURN:
                //aktor yang bermain sekarang berganti ke karakter enemy sesuai urutan index.
                //
                currentUnitPlay = enemyUnit[enemyIndex];
                //menonaktifkan seluruh button yang ada pada game
                //
                Actions.IsDisableAllButton?.Invoke(true);
                //mengganti dialog teks yang sesuai dibutuhkan
                //
                DialogText.text = "Enemy Turn ! " + currentUnitPlay._character.charaData.unitName;
                //karakter yang berjalan akan mempunyai highlight kuning
                //
                currentUnitPlay.transform.GetChild(0).GetComponentInChildren<SpriteRenderer>().color = Color.yellow;
                // pemberian if statement jika turn sekarang lebih tinggi dari last turn dan jika karakter sedang defense up, maka defense dikembalikan ke normal
                //
                currentUnitPlay.CurrentTurn++;
                // pemberian if statement jika turn sekarang lebih tinggi dari last turn dan jika karakter sedang defense up, maka defense dikembalikan ke normal
                //
                if (currentUnitPlay.CurrentTurn > currentUnitPlay.LastTurn)
                {
                    if (currentUnitPlay.isdefup == true)
                    {
                        currentUnitPlay.DefDefault();
                    }
                }
                //menjalankan aksi enemy secara otomatis dengan menjalankan korotin
                //
                StartCoroutine(EnemyAttack());
                break;
            case BattleState.WON:
                //jika fungsi wining sudah dipanggil maka tidak akan berjalan lagi
                //
                if (winning) return;
                //jike belum maka wining = benar
                //
                winning = true;
                Debug.Log("Won");
                //mengganti dialog teks yang sesuai dibutuhkan
                //
                DialogText.text = "Player Win The Battle!";
                //menyalakan musik kemenangan
                //
                AudioManager.instance.Play("Win");
                //Memberikan sinyal OnQuestFinish agar QuestManager menangkap sinyal tersebut lalu menjalankan perintah sesuai dengan sinyal tersebut
                //
                Actions.onQuestFinish?.Invoke(FindObjectOfType<GameManager>().currentQuest);
                //mendelay game selama 2 detik
                //
                await Task.Delay(2000);
                //memberikan sinyal OnResultBattle dengan parameter boolean yang dimana jika true maka result battle = win
                //
                Actions.OnResultBattle?.Invoke(true);
                break;
            case BattleState.LOST:
                //jika fungsi loosing sudah dipanggil maka tidak akan berjalan lagi
                //
                if (loosing) return;
                //jike belum maka wining = benar
                //
                loosing = true;
                Debug.Log("Lost");
                //menyalakan musik kekalahan
                //
                AudioManager.instance.Play("Lose");
                //mengganti dialog teks yang sesuai dibutuhkan
                //
                DialogText.text = "Enemy Win The Battle!";
                //mendelay game selama 2 detik
                //
                await Task.Delay(2000);
                //memberikan sinyal OnResultBattle dengan parameter boolean yang dimana jika false maka result battle = lose
                //
                Actions.OnResultBattle?.Invoke(false);
                break;
            case BattleState.CHECK:
                //if statement jika semua enemy unit habis maka state diubah ke win
                //
                if (enemyUnit == null || enemyUnit.Count <= 0)
                {
                    ChangeState(BattleState.WON);
                    return;
                }
                //if statement jika semua player unit habis maka state diubah ke lose
                //
                if (playerUnit == null || playerUnit.Count <= 0)
                {
                    ChangeState(BattleState.LOST);
                    return;
                }
                //if statement jika semua player sudah berjalan maka pindah ke enemy turn
                //
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
                //if statement jika semua enemy sudah berjalan maka pindah ke player turn
                //
                if (!playerTurnIsDone && enemyTurnIsDone)
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
