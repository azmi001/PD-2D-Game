using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
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
    private async void OnQuestFinish(StoryQuest quest)
    {
        //unlock next quest
        for (int i = 0; i < ListQuest.Length; i++) 
        {
            if (ListQuest[i].QuestName == quest.QuestName)
            {
                try
                {
                    i++;
                    ListQuest[i].UnlockQuest = true;
                    PlayerPrefs.SetInt(ListQuest[i].QuestName, ListQuest[i].UnlockQuest == true ? 1 : 0);
                    break;
                }
                catch
                {
                    continue;
                }
            }
        }
        foreach (var item in quest.listQuestReward)
        {
            await Task.Delay(500); // jeda 0.5 detik
            Debug.Log(item.rewardType);
            switch (item.rewardType)
            {
                case QuestReward.RewardType.Character:
                    Funcs.GetAkun().AddHeroes(item.rewardName);
                    break;
                case QuestReward.RewardType.Currencies:
                    Debug.Log("budi");
                    Funcs.GetAkun().AddHeroesExp((float)quest.heroesExpReward);
                    break;
                default:
                    break;
            }
        }
    }
}
