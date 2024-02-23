using UnityEngine;

[CreateAssetMenu(fileName = "New Skill", menuName = "Skill")]
public class Skill : ScriptableObject
{
    [Header("Nama Skill")]
    public string skillName;

    [Header("Skill Atribute")]
    public int skillDmg;
    public int skillHeal;
    public int skillDef;

    /*[Header("Icon Skill")]
    public Sprite icon;*/
}
