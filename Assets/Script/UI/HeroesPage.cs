using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HeroesPage : MonoBehaviour
{
    public Transform ContentParrent;
    public GameObject CharacterPrefabs;

    private void Start()
    {
        foreach (var item in Funcs.GetAkun().OwnedHeroes)
        {
            if (Funcs.GetDatabaseSOCharacter().GetCharacter(item.unitName) != null)
            {
                GameObject go = Instantiate(CharacterPrefabs, ContentParrent);
                go.GetComponentInChildren<TMP_Text>().text = Funcs.GetDatabaseSOCharacter().GetCharacter(item.unitName).charaData.unitName;
                go.GetComponent<Image>().sprite = Funcs.GetDatabaseSOCharacter().GetCharacter(item.unitName).HeroIcon;
            }
        }
    }
}