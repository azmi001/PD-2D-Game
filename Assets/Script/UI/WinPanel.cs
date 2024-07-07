using System;
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

            nextBTN.onClick.AddListener(() => {
                SceneManager.LoadScene("Hub");
            });
        }
        for (int i = 0; i < Funcs.GetAkun().teamHeroes.Count; i++)
        {
            try
            {
                CharacterData target = Array.Find(Funcs.GetAkun().OwnedHeroes.ToArray(), t => t.unitName == Funcs.GetAkun().teamHeroes[i]);
                lvTexts[i].text = $"Level : {target.unitLevel}";
                int targetExp = (int)Mathf.Pow(target.unitLevel / 0.09f, 1.3f);
                expSliders[i].maxValue = targetExp;
                expSliders[i].value = target.unitexp;
                heroesIcon[i].sprite = Funcs.GetDatabaseSOCharacter().GetCharacter(target.unitName).HeroIcon;
            }
            catch
            {
                continue;
            }
        }
    }
}
