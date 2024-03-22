using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    //penyimapan variable data story quest
    StoryQuest currentQuest;

    //Nama Scene yang mau di load
    public string scenename;

    Akun akun;

    private void Awake()
    {
        akun = new Akun();
        akun.akunLvl = 1;
        akun.akunStamina = 100;
        akun.akunExp = 0;
        akun.akunMoney = 0;
        DontDestroyOnLoad(gameObject);
    }

    private void OnEnable()
    {
        Actions.onQuestStart += StartQuest;
        Funcs.GetAkun += GetAccount;
    }

    private Akun GetAccount()
    {
        return akun;
    }

    private void OnDisable()
    {
        Actions.onQuestStart -= StartQuest;
        Funcs.GetAkun -= GetAccount;
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
        SceneManager.LoadScene(scenename);
    }
}
