using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSelection : MonoBehaviour
{
    private Image Temp;

    public int _index;

    public Image CharacterImage1;
    public Image CharacterImage2;
    public Image CharacterImage3;

    public Button ChooseCharacterButton1;
    public Button ChooseCharacterButton2;
    public Button ChooseCharacterButton3;

    List<Button> CharacterButton = new List<Button>();

    public Unit[] CharacterName;
    public Transform ContentParrent;
    public GameObject CharacterPrefabs;

    private void Start()
    {
        for (int i = 0; i < CharacterName.Length; i++)
        {
            GameObject go = Instantiate(CharacterPrefabs, ContentParrent);
            go.GetComponentInChildren<TMP_Text>().text = CharacterName[i].character.unitName;
            go.GetComponent<Image>().sprite = CharacterName[i].character.HeroIcon;

            go.GetComponent<Button>().onClick.AddListener(() => SelectTarget(go.transform.GetSiblingIndex()));

            CharacterButton.Add(go.GetComponent<Button>());
        }

        ContentParrent.gameObject.SetActive(false);

        ChooseCharacterButton1.onClick.AddListener(() => 
        {
            ContentParrent.gameObject.SetActive(true);

            Temp = CharacterImage1;
            _index = 0;
        });

        ChooseCharacterButton2.onClick.AddListener(() =>
        {
            ContentParrent.gameObject.SetActive(true);

            Temp = CharacterImage2;
            _index = 1;
        });

        ChooseCharacterButton3.onClick.AddListener(() =>
        {
            ContentParrent.gameObject.SetActive(true);

            Temp = CharacterImage3;
            _index = 2;
        });
    }

    private void SelectTarget(int index)
    {
        if (Temp.sprite != null)
        {
            foreach (var item in CharacterButton)
            {
                if (item.GetComponent<Image>().sprite == Temp.sprite)
                {
                    item.GetComponent<Button>().interactable = true;
                }
            }
        }

        Temp.sprite = CharacterName[index].character.HeroIcon;

        ContentParrent.gameObject.SetActive(false);

        Funcs.GetAkun().AddTeam(CharacterName[index].gameObject, _index);
        CharacterButton[index].interactable = false;
    }
}
