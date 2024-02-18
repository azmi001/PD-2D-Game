using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Character", menuName = "Character")]
public class Character : ScriptableObject
{
    [Header("Unit Stat")]
    public string unitName;
    public int unitLevel;
    public int deffense;
    public int damage;
    public int maxHP;

    public enum ElementType
    {
        Fire,
        Leaf,
        Water
    }

    [Header("Element Unit")]
    public ElementType thisUnitElement;
}
