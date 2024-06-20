using UnityEngine;

[System.Serializable]
public class CharacterData
{
    public string unitName;
    public int unitLevel;
    public float unitexp;
    public int deffense;
    public int damage;
    public int maxHP;
    public int Heal;

    public void AddExp(float amount)
    {
        int targetExp = (int)Mathf.Pow(unitLevel / 0.09f, 1.6f);
        unitexp += amount;
        while (unitexp > targetExp)
        {
            float expRemain = unitexp - targetExp;
            unitexp = expRemain;
            unitLevel++;
            targetExp = (int)Mathf.Pow(unitLevel / 0.09f, 1.6f);
        }
    }
}