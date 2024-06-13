using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    //penyimapan variable data story quest
    public StoryQuest currentQuest;

    //Nama Scene yang mau di load
    public string scenename;

    //
    public Character character;

    [SerializeField] Akun akun;

    private void Awake()
    {
        akun = new Akun();
        akun.akunLvl = 1;
        akun.akunStamina = 100;
        akun.akunExp = 0;
        akun.akunMoney = 0;

        //character.unitName = "ucup";

        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        
    }

    private void OnEnable()
    {
        Actions.onQuestStart += StartQuest;
        Funcs.GetAkun += GetAccount;
        Actions.onQuestFinis += OnQuestFinish;
    }

    //melakukan pengecekan quest complete atau tidak
    private void OnQuestFinish(StoryQuest quest)
    {
        quest.UnlockQuest = true;
        PlayerPrefs.SetInt(quest.QuestName, quest.UnlockQuest == true?1:0);
    }

    private Akun GetAccount()
    {
        return akun;
    }

    private void OnDisable()
    {
        Actions.onQuestStart -= StartQuest;
        Funcs.GetAkun -= GetAccount;
        Actions.onQuestFinis -= OnQuestFinish;
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
