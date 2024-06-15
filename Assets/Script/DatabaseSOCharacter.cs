using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DatabaseSOCharacter : MonoBehaviour
{
    [SerializeField] private Character[] listCharacter;
    public Character[] GetListCharacter()
    {
        return listCharacter;
    }
    public Character GetCharacter(string byName)
    {
        Character temp = Array.Find(listCharacter, t => t.charaData.unitName == byName);
        return temp;
    }
}
