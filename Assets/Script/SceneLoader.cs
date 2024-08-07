using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoader : MonoBehaviour
{
    public string scenename;
    public StoryQuest Quest;
    private void Start()
    {
        if (Quest == null) return;
        if (Quest.UnlockQuest)
        {
            GetComponent<Button>().interactable = true;
        }
        else
        {
            GetComponent<Button>().interactable = false;
        }

        GetComponent<Button>().onClick.AddListener(() => LoadScene(Quest));
    }
    public void LoadScene(StoryQuest quest)
    {
        Actions.onQuestStart.Invoke(quest);
    }
}
