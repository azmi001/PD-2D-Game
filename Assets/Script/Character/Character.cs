using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Character", menuName = "Character")]
public partial class Character : ScriptableObject
{
    // atribute stast character
    [Header("Unit Stat")]
    public CharacterData charaData;
    public bool Unlock;

    public Sprite HeroIcon;
    public Sprite heroIconFullBody;

    //Character Biography
    [Header("Character Bio")]
    [TextArea(5,3)]
    public string chBio;
    

    // mendefinisikan tipe element character
    [Header("Element Unit")]
    public ElementType thisUnitElement;

    [Header("List Skill Character")]
    public List<Skill> skills;
}
