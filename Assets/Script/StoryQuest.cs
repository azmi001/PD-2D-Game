using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Story Quest", menuName = "Quest")]
public class StoryQuest : ScriptableObject
{
    public string QuestName;
    public GameObject[] EnemyPrefabs;
    public int money;
    public int exp;
    public int staminaCost;
}
