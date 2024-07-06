using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using static UnityEditor.Progress;

[System.Serializable]
public class Akun
{
    public int akunLvl;
    public int akunExp;
    public int akunStamina;
    public int akunStaminaMax = 10;
    public int akunMoney;

    public List<CharacterData> OwnedHeroes = new();
    public List<string> teamHeroes = new();
    public void AddTeam(string character, int index)
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
        JsonHelper.SaveToJSON(this, "Akun");
    }
    public void AddHeroes(string heroName)
    {
        CharacterData target = Funcs.GetDatabaseSOCharacter().GetCharacter(heroName).charaData;
        Debug.Log(target == null);
        if (target != null)
        {
            CharacterData checkOwn = Array.Find(OwnedHeroes.ToArray(), c => c.unitName == target.unitName);
            if (checkOwn == null)
            {
                Debug.Log("Character Ditambahkan");
                OwnedHeroes.Add(target);
            }
            else
            {
                Debug.Log("Character tidak ditambahkan karena sudah dimiliki");
            }

        }
        JsonHelper.SaveToJSON(this, "Akun");
    }
    public void RemoveHeroFromTeam(string character)
    {
        string target = Array.Find(teamHeroes.ToArray(),t=>t == character);
        if(!string.IsNullOrEmpty(target))
            teamHeroes.Remove(target);
        JsonHelper.SaveToJSON(this, "Akun");
    }
    public void AddHeroesExp(float heroesExpReward)
    {
        foreach (var item in teamHeroes) // for loop nama nama hero di tim
        {
            var target = Array.Find(OwnedHeroes.ToArray(), t => t.unitName == item); // mencari target dari owned heroes yang bernama sama dengan forloop item
            if (target != null)
            {
                target.AddExp(heroesExpReward);
            }
        }
        //menyimpan data hero ke akun
        JsonHelper.SaveToJSON(this, "Akun");
    }
    public void AddStamina(int amount)
    {
        akunStamina = Mathf.Clamp(akunStamina + amount, 0, akunStaminaMax);
        // Menyimpan data stamina
        JsonHelper.SaveToJSON(this, "Akun");
    }
}
