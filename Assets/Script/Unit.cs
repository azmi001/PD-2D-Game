using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;

public class Unit : MonoBehaviour
{
    //data darah yang sekarang akan terupdate dalam game
    public int currentHP;

    //Merenfrensikan dari scriptable object Character untuk mengambil data stat character 
    public Character character;

    //inisialisasi awal darah
    public void Awake()
    {
        currentHP = character.maxHP;
    }

    //logika penyerangan
    public bool TakeDemage(int dmg, int def, ElementType attackerElement)
    {
        //Mendubug dmg awal
        Debug.Log("Demage Murni " + character.unitName + "yang belum dicampur elemen " + dmg);

        //inisialisasi awal logika sitem dmg elemen
        int actualDamage = dmg * 15/100;
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
        int varian = finalDmg * 20 / 100;
        int minVarian = -varian;
        int maxVarian = varian;
        int result = Random.Range(minVarian, maxVarian);

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
}
