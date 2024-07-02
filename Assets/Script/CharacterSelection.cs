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

    public Transform ContentParrent;
    public GameObject CharacterPrefabs;

    List<Character> tempListCharacter = new();

    public Sprite imageHolder;

    private void Awake()
    {
        for (int i = 0; i < Funcs.GetDatabaseSOCharacter().GetListCharacter().Length; i++)
        {
            if (Funcs.GetDatabaseSOCharacter().GetListCharacter()[i].Unlock)
            {
                GameObject go = Instantiate(CharacterPrefabs, ContentParrent);
                go.GetComponentInChildren<TMP_Text>().text = Funcs.GetDatabaseSOCharacter().GetListCharacter()[i].charaData.unitName;
                go.GetComponent<Image>().sprite = Funcs.GetDatabaseSOCharacter().GetListCharacter()[i].HeroIcon;

                go.GetComponent<Button>().onClick.AddListener(() => SelectTarget(go.transform.GetSiblingIndex()));
                tempListCharacter.Add(Funcs.GetDatabaseSOCharacter().GetListCharacter()[i]);

                CharacterButton.Add(go.GetComponent<Button>());

            }
        }

        CharacterImage1.GetComponent<Button>().onClick.AddListener(() =>
        {
            Temp = CharacterImage1;
            _index = 0;
            UnSelectTarget(_index);
        });
        CharacterImage2.GetComponent<Button>().onClick.AddListener(() =>
        {
            Temp = CharacterImage2;
            _index = 1;
            UnSelectTarget(_index);
        });
        CharacterImage3.GetComponent<Button>().onClick.AddListener(() =>
        {
            Temp = CharacterImage3;
            _index = 2;
            UnSelectTarget(_index);
        });


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
    private void OnEnable()
    {
        RefreshUI();
    }

    private void RefreshUI()
    {
        if (Funcs.GetAkun().teamHeroes.Count <= 0)
            return;


        if (Funcs.GetAkun().teamHeroes.Count == 1)
        {
            for (int i = 0; i < tempListCharacter.Count; i++)
            {
                try
                {
                    if (tempListCharacter[i].charaData.unitName == Funcs.GetAkun().teamHeroes[0])
                    {
                        CharacterImage1.sprite = tempListCharacter[i].HeroIcon;
                        CharacterButton[i].interactable = false;
                        break;
                    }
                }
                catch
                {
                    continue;
                }
            }
        }
        if (Funcs.GetAkun().teamHeroes.Count == 2)
        {
            for (int i = 0; i < tempListCharacter.Count; i++)
            {
                try
                {
                    if (tempListCharacter[i].charaData.unitName == Funcs.GetAkun().teamHeroes[0])
                    {
                        CharacterImage1.sprite = tempListCharacter[i].HeroIcon;
                        CharacterButton[i].interactable = false;
                        break;
                    }
                }
                catch
                {
                    continue;
                }
            }
            for (int i = 0; i < tempListCharacter.Count; i++)
            {
                try
                {
                    if (tempListCharacter[i].charaData.unitName == Funcs.GetAkun().teamHeroes[1])
                    {
                        CharacterImage2.sprite = tempListCharacter[i].HeroIcon;
                        CharacterButton[i].interactable = false;
                        break;
                    }
                }
                catch
                {
                    continue;
                }
            }

        }
        if (Funcs.GetAkun().teamHeroes.Count == 3)
        {
            for (int i = 0; i < tempListCharacter.Count; i++)
            {
                try
                {
                    if (tempListCharacter[i].charaData.unitName == Funcs.GetAkun().teamHeroes[0])
                    {
                        CharacterImage1.sprite = tempListCharacter[i].HeroIcon;
                        CharacterButton[i].interactable = false;
                        break;
                    }
                }
                catch
                {
                    continue;
                }
            }
            for (int i = 0; i < tempListCharacter.Count; i++)
            {
                try
                {
                    if (tempListCharacter[i].charaData.unitName == Funcs.GetAkun().teamHeroes[1])
                    {
                        CharacterImage2.sprite = tempListCharacter[i].HeroIcon;
                        CharacterButton[i].interactable = false;
                        break;
                    }
                }
                catch
                {
                    continue;
                }
            }
            for (int i = 0; i < tempListCharacter.Count; i++)
            {
                try
                {
                    if (tempListCharacter[i].charaData.unitName == Funcs.GetAkun().teamHeroes[2])
                    {
                        CharacterImage3.sprite = tempListCharacter[i].HeroIcon;
                        CharacterButton[i].interactable = false;
                        break;
                    }
                }
                catch
                {
                    continue;
                }
            }
        }
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

        Temp.sprite = tempListCharacter[index].HeroIcon;

        ContentParrent.gameObject.SetActive(false);

        Funcs.GetAkun().AddTeam(tempListCharacter[index].charaData.unitName, _index);
        CharacterButton[index].interactable = false;
    }
    void UnSelectTarget(int index)
    {
        if (Temp.sprite != null)
        {
            Temp.sprite = imageHolder;
            CharacterButton[index].interactable = true;
            Funcs.GetAkun().RemoveHeroFromTeam(tempListCharacter[index].charaData.unitName);
        }
    }
}
