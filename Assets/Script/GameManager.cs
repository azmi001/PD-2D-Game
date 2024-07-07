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

    public int StaminaGet = 1;
    private Akun GetAccount()
    {
        return akun;
    }

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

        //load akun
        akun = JsonHelper.ReadFromJSON<Akun>("Akun");
        if (akun == null)
        {
            akun = new Akun();
            akun.akunLvl = 1;
            akun.akunStamina = akun.akunStaminaMax;
            akun.akunExp = 0;
            akun.akunMoney = 0;
            foreach (var item in GetDatabaseSOCharacter().GetListCharacter())
            {
                Debug.Log(item.charaData.Unlock);
                if (item.charaData.Unlock)
                {
                    akun.OwnedHeroes.Add(item.charaData);
                }
            }
            JsonHelper.SaveToJSON(akun, "Akun");
        }

    }
    private void Start()
    {
        //Buat load Data
        foreach (var item in akun.OwnedHeroes)
        {
            item.Init();
            Character target = ScriptableObject.Instantiate(Funcs.GetDatabaseSOCharacter().GetCharacter(item.unitName));
            target.charaData = item;
            if (target == null) return;
            target.LoadData(item);
        }
    }
    private void OnEnable()
    {
        Actions.onQuestStart += StartQuest;
        Actions.ClaimStamina += OnClaimStamina;
        Funcs.GetAkun += GetAccount;
        Funcs.GetDatabaseSOCharacter += GetDatabaseSOCharacter;
        Funcs.GetDatabaseUnit += GetDatabaseUnit;
        Funcs.GetCurrentQuest += GetCurrentQuest;
        Funcs.GetCountdownStamina += GetCountdownStamina;
    }

    private void OnDisable()
    {
        Actions.onQuestStart -= StartQuest;
        Actions.ClaimStamina -= OnClaimStamina;
        Funcs.GetAkun -= GetAccount;
        Funcs.GetDatabaseSOCharacter -= GetDatabaseSOCharacter;
        Funcs.GetDatabaseUnit -= GetDatabaseUnit;
        Funcs.GetCurrentQuest -= GetCurrentQuest;
        Funcs.GetCountdownStamina -= GetCountdownStamina;

        foreach (var item in akun.OwnedHeroes)
        {
            item.ResetData();
        }
    }

    private DateTime GetCountdownStamina()
    {
        DateTime.TryParse(PlayerPrefs.GetString("ClaimedStaminaDate"),out var result);
        return result;
    }

    private void OnClaimStamina(Action OnClaimed)
    {
        bool canClaim = false;
        if (PlayerPrefs.HasKey("ClaimedStaminaDate"))
        {
            if (DateTime.Now > DateTime.Parse(PlayerPrefs.GetString("ClaimedStaminaDate")))
            {
                canClaim = true;
            }
            else
            {
                canClaim = false;
            }
        }
        else
        {
            canClaim = true;
        }
        if (canClaim)
        {
            string claimedStaminaDate = DateTime.Now.AddHours(2).ToString();
            PlayerPrefs.SetString("ClaimedStaminaDate", claimedStaminaDate);
            akun.AddStamina(StaminaGet);
            Debug.Log("Stamina up +" + akun.akunStamina);
        }
        else
        {
            TimeSpan timeSpan = DateTime.Now - DateTime.Parse(PlayerPrefs.GetString("ClaimedStaminaDate"));
            Debug.Log("you can claim after " + timeSpan);
        }
        OnClaimed.Invoke();

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
