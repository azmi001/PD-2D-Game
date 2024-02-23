using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Character", menuName = "Character")]
public partial class Character : ScriptableObject
{
    // atribute stast character
    [Header("Unit Stat")]
    public string unitName;
    public int unitLevel;
    public float unitexp;
    public int deffense;
    public int damage;
    public int maxHP;

    // mendefinisikan tipe element character
    [Header("Element Unit")]
    public ElementType thisUnitElement;

    [Header("List Skill Character")]
    public List<Skill> skills;
}
