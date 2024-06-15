using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DatabaseUnit : MonoBehaviour
{
    [SerializeField] private Unit[] listUnit;

    public Unit[] GetListUnit()
    {
        return listUnit;
    }
    public Unit GetUnit(string byName)
    {
        Unit temp = Array.Find(listUnit, t => t.character.charaData.unitName == byName);
        return temp;
    }
}
