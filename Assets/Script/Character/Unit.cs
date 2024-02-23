using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;

public class Unit : MonoBehaviour
{
    //data darah yang sekarang akan terupdate dalam game
    [Header("Status Darah")]
    public int currentHP;

    [Header("Status Level Charcter")]
    public float currentXP;
    public float expToLvUp;
    public float currentLv;

    //Merenfrensikan dari scriptable object Character untuk mengambil data stat character
    [Header("Data Character")]
    public Character character;

    [Header("List Skill Character")]
    public List<Skill> skillList;

    //inisialisasi awal darah
    public void Awake()
    {
        InitializedData();
    }

    public void Start()
    {
        float hasil = MathF.Pow(2f / 0.09f, 1.6f);
        Debug.Log(hasil);
    }

    public void InitializedData()
    {
        currentHP = character.maxHP;
        currentLv = character.unitLevel;
        currentXP = character.unitexp;
        skillList = new List<Skill>(character.skills);
    }

    //logika penyerangan
    public bool TakeDemage(int dmg, int def, ElementType attackerElement)
    {
        //Mendubug dmg awal
        Debug.Log("Demage Murni " + character.unitName + "yang belum dicampur elemen " + dmg);

        //inisialisasi awal logika sitem dmg elemen
        int actualDamage = dmg * BattleSystem.instance.elDmg /100;
        Debug.Log("Demage elemen didapat " + character.unitName + "adalah " + actualDamage);

        switch (character.thisUnitElement)
        {
            case ElementType.Fire:
                if (attackerElement == ElementType.Leaf)
                    actualDamage *= -1; // Double damage
                else if (attackerElement == ElementType.Water)
                    actualDamage *= 1; // Half damage
                break;
            case ElementType.Leaf:
                if (attackerElement == ElementType.Water)
                    actualDamage *= -1; // Double damage
                else if (attackerElement == ElementType.Fire)
                    actualDamage *= 1; // Half damage
                break;
            case ElementType.Water:
                if (attackerElement == ElementType.Fire)
                    actualDamage *= -1; // Double damage
                else if (attackerElement == ElementType.Leaf)
                    actualDamage *= 1; // Half damage
                break;
        }

        //total dmg yang sudah ditambah dari dmg element
        int finalDmg = dmg + actualDamage;

        //mendebug total dmg final dmg
        Debug.Log("Demage Final " + character.unitName + "yang dicampur elemen " + finalDmg);

        //mebuat logika variasi dmg 20% +- dari total finaldmg
        int varian = finalDmg * BattleSystem.instance.varDmg / 100;
        int minVarian = -varian;
        int maxVarian = varian;
        int result = UnityEngine.Random.Range(minVarian, maxVarian);

        //mendebug nial variasi dmg varian
        Debug.Log(character.unitName + "Min range -varian dari dmg " + minVarian);
        Debug.Log(character.unitName + "Min range +varian dari dmg " + maxVarian);
        Debug.Log(character.unitName +"Variasi dmg tambahan +- " + result);

        //mendebug nilai finaldmg yang ditambah oleh nilai variasi 
        int totalDmg = finalDmg + result;
        Debug.Log("Total Demage yang diberikan oleh " + character.unitName + "adalah " + totalDmg);

        //logika rumus pengurangan darah target 
        int finalDmg1 = ((finalDmg + result) * 4);
        int def1 = (def * 2);

        //menbuat logika jika dmg nya minus gak akan menambah darah target yang diserang
        //dan dmg yang diterima adalah 0
        if (def1 >= finalDmg1)
        {
            def1 = finalDmg1;
        }

        currentHP -= finalDmg1 - def1;
        //currentHP -= ((finalDmg + result) * 4) - (def * 2);

        //pengkondisian apakah target yang diserah sudah mati atau belum
        if (currentHP <= 0)
            return true;
        else 
            return false;
    }
    
    //logika skill heal
    public void Heal(int amount)
    {
        currentHP += amount;
        if (currentHP > character.maxHP)
            currentHP = character.maxHP;
    }
    

    public void GainExp(int exp)
    {
        currentXP += exp;
        if(currentXP >= expToLvUp)
        {
            LvUp();
        }
    }

    public void LvUp()
    {
        currentLv++;
        currentXP -= expToLvUp; 
        expToLvUp = CalculateNextLevelXP();
    }

    float CalculateNextLevelXP()
    {
        float result = Mathf.Pow(currentLv + 1 / 0.09f, 1.6f);
        Debug.Log("Result: " + result);
        return result;
    }
}
