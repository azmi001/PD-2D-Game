using System.Collections;
using UnityEngine;
using Unity.Collections;
using System;
#if UNITY_EDITOR
using UnityEditor;
#endif

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
    public abstract IEnumerator ActionSkill();

    /*[Header("Icon Skill")]
    public Sprite icon;*/
}