using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;

public class Unit : MonoBehaviour
{
    [Header("Unit Stat")]
    public string unitName;
    public int unitLevel;
    public int deffense;
    public int damage;
    public int bp;
    public int maxHP;
    public int currentHP;

    public enum ElementType
    {
        Fire,
        Leaf,
        Water
    }

    [Header("Element Unit")]
    public ElementType thisUnitElement;

    public bool TakeDemage(int dmg, int def, ElementType attackerElement)
    {
        //int finaldmg;
        /*int finaldmg;
        int temp;
        temp = (dmg + ul) / (1 + (def / 100));
        finaldmg = bp * temp;*/

        /*finaldmg = Mathf.RoundToInt(bp * ((dmg + ul) / (1 + (def / 100f))));

        Debug.Log(finaldmg);

        if (def >= finaldmg)
        {
            def = finaldmg;
        }

        currentHP -= finaldmg;

        if (currentHP <= 0)
            return true;
        else
            return false;*/

        //float dmgMultiplier = 1f;

        Debug.Log(dmg);
        //Debug.Log(def);

        /*if (ElementSystem.IsStrongAgainst(playerType, enemyType))
        {
            dmgMultiplier = 2f;
        } else if (ElementSystem.IsWeakAgainst(playerType, enemyType))
        {
            dmgMultiplier = 0.5f;
        } else
        {
            dmgMultiplier = 1f;
        }*/

        /*int percentage = Random.Range(80,100);*/

        int actualDamage = dmg /* * (percentage / 100)*/;


        switch (thisUnitElement)
        {
            case ElementType.Fire:
                if (attackerElement == ElementType.Leaf)
                    actualDamage /= 2; // Double damage
                else if (attackerElement == ElementType.Water)
                    actualDamage *= 2; // Half damage
                break;
            case ElementType.Leaf:
                if (attackerElement == ElementType.Water)
                    actualDamage /= 2; // Double damage
                else if (attackerElement == ElementType.Fire)
                    actualDamage *= 2; // Half damage
                break;
            case ElementType.Water:
                if (attackerElement == ElementType.Fire)
                    actualDamage /= 2; // Double damage
                else if (attackerElement == ElementType.Leaf)
                    actualDamage *= 2; // Half damage
                break;
        }

        int finalDmg = actualDamage;

        def *= 2;

        if (def >= finalDmg)
        {
            def = finalDmg;
        }

        Debug.Log(finalDmg);
        //Debug.Log(def);

        currentHP -= (finalDmg - def);

        if (currentHP <= 0)
            return true;
        else 
            return false;
    }

    public void Heal(int amount)
    {
        currentHP += amount;
        if (currentHP > maxHP)
            currentHP = maxHP;
    }
}
