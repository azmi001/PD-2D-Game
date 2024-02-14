using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public string unitName;
    public int unitLevel;
    public int deffense;
    public int damage;

    public int maxHP;
    public int currentHP;

    public bool TakeDemage(int dmg,int def)
    {
        if (def >= dmg)
        {
            def = dmg;
        }
        currentHP -= (dmg - def);

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
