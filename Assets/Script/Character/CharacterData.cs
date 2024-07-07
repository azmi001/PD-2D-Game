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
    public bool Unlock;

    public void Init()
    {
        //PlayerPrefs.SetString(unitName, unitName);
        //PlayerPrefs.SetInt("unitLevel", unitLevel);
        //PlayerPrefs.SetInt("unitUnlock", Unlock == true?1:0);
        maxHP = BaseHP + (unitLevel - 1) * GrowthRateHP;
        damage = (maxHP * BaseAttackModifier) / (ConstantAttack);
        deffense = damage * DeffenceModifier;
        Heal = maxHP * HealModifier;
    }
    public void ResetData()
    {
        //unitName = PlayerPrefs.GetString(unitName, unitName);
        //unitLevel = PlayerPrefs.GetInt("unitLevel",1);
        //unitexp = 0;
        //Unlock = PlayerPrefs.GetInt("unitUnlock")==1?true:false;
    }
    public void AddExp(float amount)
    {
        int targetExp = (int)Mathf.Pow(unitLevel / 0.09f, 1.3f);
        unitexp += amount;
        Debug.Log(amount + " Exp yg didapat");
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
            Debug.Log(targetExp + " Next Exp yang didapat");
            Debug.Log(expRemain + " Sisa Exp yang didapat");
        }
    }
}