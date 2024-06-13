using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public StoryQuest[] ListQuest;


    // Start is called before the first frame update
    void Start()
    {
        foreach (StoryQuest s in ListQuest)
        {
            if (PlayerPrefs.HasKey(s.QuestName))
            {
                s.UnlockQuest = PlayerPrefs.GetInt(s.QuestName) == 1 ? true : false;
            }
            
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
