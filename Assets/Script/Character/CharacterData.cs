using UnityEngine;

[System.Serializable]
public class CharacterData
{
    [Header("Base Value")]
    public float BaseHP = 200;
    public float GrowthRateHP = 150;
    public float BaseAttackModifier = 0.05f;
    public float ConstantAttack = 0.4f;
    public float DeffenceModifier = 0.4f;
    public float HealModifier = 0.06f;

    [Header("Data Berubah")]
    public string unitName;
    public int unitLevel;
    public float unitexp;
    public float deffense;
    public float damage;
    public float maxHP;
    public float Heal;

    public void Init()
    {
        PlayerPrefs.SetString(unitName, unitName);
        PlayerPrefs.SetInt("unitLevel", unitLevel);

        maxHP = BaseHP + (unitLevel - 1) * GrowthRateHP;
        damage = (maxHP * BaseAttackModifier) / (ConstantAttack);
        deffense = damage * DeffenceModifier;
        Heal = maxHP * HealModifier;
    }
    public void ResetData()
    {
        unitName = PlayerPrefs.GetString(unitName, unitName);
        unitLevel = PlayerPrefs.GetInt("unitLevel",1);
    }
    public void AddExp(float amount)
    {
        int targetExp = (int)Mathf.Pow(unitLevel / 0.09f, 1.3f);
        unitexp += amount;
        while (unitexp > targetExp)
        {
            float expRemain = unitexp - targetExp;
            unitexp = expRemain;
            unitLevel++;
            //rumus naikin stats char
            maxHP = BaseHP + (unitLevel - 1) * GrowthRateHP;
            damage = (maxHP * BaseAttackModifier) / (ConstantAttack);
            deffense = damage * DeffenceModifier;
            Heal = maxHP * HealModifier;

            targetExp = (int)Mathf.Pow(unitLevel / 0.09f, 1.3f);
        }
    }
}