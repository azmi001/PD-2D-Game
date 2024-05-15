using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Akun
{
    public int akunLvl;
    public int akunExp;
    public int akunStamina;
    public int akunMoney;

    public List<Character> heroes = new();
    //public List<Character> teamHeroes = new();

    public GameObject TeamHeroes1;
    public GameObject TeamHeroes2;
    public GameObject TeamHeroes3;

    public void AddTeam(GameObject character, int index)
    {
        if (index == 0) TeamHeroes1 = character;
        if (index == 1) TeamHeroes2 = character;
        if (index == 2) TeamHeroes3 = character;
    }
}
