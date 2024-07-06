using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HeroesPage : MonoBehaviour
{
    public GameObject heroView;
    public Transform ContentParrent;
    public GameObject CharacterPrefabs;

    public GameObject HeroDescriptionPanel;
    public Image heroPlaceholder;
    public TMP_Text heroStatsInfo;

    public Button backBTN;
    private void Start()
    {
        backBTN.onClick.AddListener(() =>
        {
            if (HeroDescriptionPanel.activeInHierarchy)
            {
                HeroDescriptionPanel.SetActive(false);
                heroView.SetActive(true);
            }
            else
            {
                gameObject.SetActive(false);
            }
        });
        foreach (var item in Funcs.GetAkun().OwnedHeroes)
        {
            if (Funcs.GetDatabaseSOCharacter().GetCharacter(item.unitName) != null)
            {
                GameObject go = Instantiate(CharacterPrefabs, ContentParrent);
                go.GetComponentInChildren<TMP_Text>().text = Funcs.GetDatabaseSOCharacter().GetCharacter(item.unitName).charaData.unitName;
                go.GetComponent<Image>().sprite = Funcs.GetDatabaseSOCharacter().GetCharacter(item.unitName).HeroIcon;
                go.GetComponent<Button>().onClick.AddListener(() =>
                {
                    OpenHeroDescription(item);
                });
            }
        }
    }

    private void OpenHeroDescription(CharacterData item)
    {
        AudioManager.instance.Play("Button");
        HeroDescriptionPanel.SetActive(true);
        heroView.SetActive(false);
        heroPlaceholder.sprite = Funcs.GetDatabaseSOCharacter().GetCharacter(item.unitName).heroIconFullBody;
        heroStatsInfo.text = $"Name : {item.unitName} \n" +
            $"Level : {item.unitLevel} \n" +
            $"Deffense : {item.deffense} \n" +
            $"Damage : {item.damage} \n" +
            $"HP : {item.maxHP} \n" +
            $"Heal : {item.Heal} \n" +
            "\n"+
            $"Description : \n {Funcs.GetDatabaseSOCharacter().GetCharacter(item.unitName).chBio}";
    }
}