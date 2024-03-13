using System.Collections;
using UnityEngine;

public abstract class Skill : ScriptableObject
{
    [Header("Nama Skill")]
    public string skillName;

    [Header("Skill Atribute")]
    public int skillDmg;
    public int skillHeal;
    public int skillDef;
    public ElementType skillElement;

    public abstract IEnumerator UseSkill(Unit target);

    /*[Header("Icon Skill")]
    public Sprite icon;*/
}