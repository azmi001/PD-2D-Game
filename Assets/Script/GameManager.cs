using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager Instance;
    //penyimapan variable data story quest
    public StoryQuest currentQuest;

    //Nama Scene yang mau di load
    public string scenename;

    //
    public Character character;

    [SerializeField] Akun akun;

    public DatabaseSOCharacter GetDatabaseSOCharacter()
    {
        DatabaseSOCharacter temp = GetComponentInChildren<DatabaseSOCharacter>();
        return temp;
    }
    public DatabaseUnit GetDatabaseUnit()
    {
        DatabaseUnit temp = GetComponentInChildren<DatabaseUnit>();
        return temp;
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        akun = JsonHelper.ReadFromJSON<Akun>("Akun");
        if (akun == null)
        {
            akun = new Akun();
            akun.akunLvl = 1;
            akun.akunStamina = 100;
            akun.akunExp = 0;
            akun.akunMoney = 0;
            foreach (var item in GetDatabaseSOCharacter().GetListCharacter())
            {
                if (item.Unlock)
                {
                    akun.OwnedHeroes.Add(item.charaData);
                }
            }
            JsonHelper.SaveToJSON(akun, "Akun");
        }
    }
    private void OnEnable()
    {
        Actions.onQuestStart += StartQuest;
        Funcs.GetAkun += GetAccount;
        Funcs.GetDatabaseSOCharacter += GetDatabaseSOCharacter;
        Funcs.GetDatabaseUnit += GetDatabaseUnit;
        Funcs.GetCurrentQuest += GetCurrentQuest;
    }

    private Akun GetAccount()
    {
        return akun;
    }

    private void OnDisable()
    {
        Actions.onQuestStart -= StartQuest;
        Funcs.GetAkun -= GetAccount;
        Funcs.GetDatabaseSOCharacter -= GetDatabaseSOCharacter;
        Funcs.GetDatabaseUnit -= GetDatabaseUnit;
        Funcs.GetCurrentQuest -= GetCurrentQuest;
    }

    private StoryQuest GetCurrentQuest()
    {
        return currentQuest;
    }

    private void StartQuest(StoryQuest quest)
    {
        currentQuest = quest;
        if (akun.akunStamina < currentQuest.staminaCost)
        {
            Debug.Log("Maaf Stamina anda tidak cukup untuk menjalankan Quest");
            return;
        }
        akun.akunStamina -= currentQuest.staminaCost;
        //SceneManager.LoadScene(scenename);
    }
}
