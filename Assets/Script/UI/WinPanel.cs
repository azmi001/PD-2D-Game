using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WinPanel : MonoBehaviour
{
    [SerializeField] private Button nextBTN, retryBTN;
    [SerializeField] private TMP_Text[] lvTexts;
    [SerializeField] private Slider[] expSliders;
    [SerializeField] private Image[] heroesIcon;

    private void Start()
    {
        retryBTN.onClick.AddListener(() => SceneManager.LoadScene(SceneManager.GetActiveScene().name));
        StoryQuest nextQuest = null;
        for (int i = 0; i < Funcs.GetAllQuest().Length; i++)
        {
            if (Funcs.GetAllQuest()[i].QuestName == Funcs.GetCurrentQuest().QuestName)
            {
                try
                {
                    i++;
                    nextQuest = Funcs.GetAllQuest()[i];

                }
                catch
                {
                    nextQuest = null;
                }
            }
        }
        if (nextQuest != null)
        {
            nextBTN.onClick.AddListener(() => {
                Actions.onQuestStart?.Invoke(nextQuest);
                SceneManager.LoadScene("Dialogue System Test");
                });
        }
        else
        {
            nextBTN.gameObject.SetActive(false);
        }

        for (int i = 0; i < lvTexts.Length; i++)
        {
            try
            {
                lvTexts[i].text = "Level : "+ Funcs.GetAllPlayerUnit()[i].character.charaData.unitLevel.ToString();
                int targetExp = (int)Mathf.Pow(Funcs.GetAllPlayerUnit()[i].character.charaData.unitLevel / 0.09f, 1.6f);
                expSliders[i].maxValue = targetExp;
                expSliders[i].value = Funcs.GetAllPlayerUnit()[i].character.charaData.unitexp;
                heroesIcon[i].sprite = Funcs.GetAllPlayerUnit()[i].character.HeroIcon;
            }
            catch 
            {
                continue;
            }
        }
    }
}
