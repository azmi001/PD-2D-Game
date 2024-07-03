using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public StoryQuest[] ListQuest;

    private void OnEnable()
    {
        Actions.onQuestFinis += OnQuestFinish;
        Funcs.GetAllQuest += GetListQuest;
    }

    private void OnDisable()
    {
        Actions.onQuestFinis -= OnQuestFinish;
        Funcs.GetAllQuest -= GetListQuest;
    }

    private StoryQuest[] GetListQuest()
    {
        return ListQuest;
    }

    // Start is called before the first frame update
    void Start()
    {
        foreach (StoryQuest s in ListQuest)
        {
            if (PlayerPrefs.HasKey(s.QuestName))
            {
                s.UnlockQuest = PlayerPrefs.GetInt(s.QuestName) == 1 ? true : false;
                Debug.Log(PlayerPrefs.GetInt(s.QuestName));
            }
        }
    }

    //melakukan pengecekan quest complete atau tidak
    private void OnQuestFinish(StoryQuest quest)
    {
        for (int i = 0; i < ListQuest.Length; i++) 
        {
            if (ListQuest[i].QuestName == quest.QuestName)
            {
                i++;
                ListQuest[i].UnlockQuest = true;
                PlayerPrefs.SetInt(ListQuest[i].QuestName, ListQuest[i].UnlockQuest == true ? 1 : 0);
                break;
            }
        }
        for (int i = 0; i < Funcs.GetAkun().OwnedHeroes.Count; i++)
        {
            try
            {
                if (Funcs.GetAkun().OwnedHeroes[i].unitName == Funcs.GetAkun().teamHeroes[i])
                {
                    Funcs.GetAkun().AddHeroesExp(Funcs.GetAkun().OwnedHeroes[i].unitName,(float)quest.heroesExpReward);
                }
            }
            catch
            {
                continue;
            }
        }
    }
}
