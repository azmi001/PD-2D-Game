using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Story Quest", menuName = "Quest")]
public class StoryQuest : ScriptableObject
{
    public string QuestName;
    public GameObject[] EnemyPrefabs;
    public int money;
    public int heroesExpReward;
    public int staminaCost;

    public bool UnlockQuest = false;

    public QuestReward[] listQuestReward;

    public Dialogue storyDialogue;
}

[System.Serializable]
public class QuestReward
{
    [Header("Reward curencies dimasukkan nama kurensi, reward character dimasukkan nama karakter")]
    public string rewardName;
    public RewardType rewardType;
    public Sprite rewardIcon;
    public enum RewardType
    {
        Character,
        Currencies
    }
}
