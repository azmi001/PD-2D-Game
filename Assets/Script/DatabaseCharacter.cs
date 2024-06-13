using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DatabaseCharacter : MonoBehaviour
{
    public Character[] CharacterName;
    public Transform ContentParrent;
    public GameObject CharacterPrefabs;

    private void Start()
    {
        for (int i = 0; i < CharacterName.Length; i++)
        {
            GameObject go = Instantiate(CharacterPrefabs, ContentParrent);
            go.GetComponentInChildren<TMP_Text>().text = CharacterName[i].unitName;
            go.GetComponent<Image>().sprite = CharacterName[i].HeroIcon;
        }

        //JsonHelper.SaveToJSON(CharacterName.ToList(), "DatabaseCharater");
    }
}
