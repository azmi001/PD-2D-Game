using UnityEngine;

[System.Serializable]
public class CharacterData
{
    [Header("Base Value")]
    public float BaseHP = 110;
    public float GrowthRateHP = 50;
    public float BaseAttackModifier = 0.05f;
    public float ConstantAttack = 0.4f;
    public float DeffenceModifier = 0.4f;
    public float HealModifier = 0.06f;

    public string unitName;
    public int unitLevel;
    public float unitexp;
    public float deffense;
    public float damage;
    public float maxHP;
    public float Heal;

    public void AddExp(float amount)
    {
        int targetExp = (int)Mathf.Pow(unitLevel / 0.09f, 1.6f);
        unitexp += amount;
        while (unitexp > targetExp)
        {
            float expRemain = unitexp - targetExp;
            unitexp = expRemain;
            unitLevel++;
            //rumus naikin stats char
            maxHP = BaseHP + (unitLevel - 1) * GrowthRateHP;
            damage = (maxHP * BaseAttackModifier) / (ConstantAttack);

            targetExp = (int)Mathf.Pow(unitLevel / 0.09f, 1.6f);
        }
    }
}