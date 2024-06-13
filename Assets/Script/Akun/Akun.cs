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

    public List<Character> OwnedHeroes = new();
    public List<GameObject> teamHeroes = new();
    public void AddTeam(GameObject character, int index)
    {
        if (teamHeroes.Count > index)
        {
            if (teamHeroes[index] != null)
            {
                teamHeroes[index] = character;
            }
        }
        else
        {
            teamHeroes.Add(character);
        }
    }
}
