using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[System.Serializable]
public class Akun
{
    public int akunLvl;
    public int akunExp;
    public int akunStamina;
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
    public void RemoveHeroFromTeam(string character)
    {
        string target = Array.Find(teamHeroes.ToArray(),t=>t == character);
        if(!string.IsNullOrEmpty(target))
            teamHeroes.Remove(target);
        JsonHelper.SaveToJSON(this, "Akun");
    }
    public async void AddHeroesExp(string heroName, float heroesExpReward)
    {
        CharacterData targetHero = Array.Find(OwnedHeroes.ToArray(), t => t.unitName == heroName);
        await Task.Run(()=>targetHero.AddExp(heroesExpReward));
        JsonHelper.SaveToJSON(this, "Akun");
    }
    public void AddStamina(int amount)
    {
        akunStamina += amount;
        JsonHelper.SaveToJSON(this, "Akun");
    }
}
