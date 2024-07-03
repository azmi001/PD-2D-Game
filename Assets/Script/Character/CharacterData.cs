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
    private string _unitName;
    public int unitLevel;
    private int _unitLevel;
    public float unitexp;
    private float _unitexp;
    public float deffense;
    private float _deffense;
    public float damage;
    private float _damage;
    public float maxHP;
    private float _maxHP;
    public float Heal;
    private float _Heal;

    public void Init()
    {
        _unitName = unitName;
        _unitLevel = unitLevel;
        _unitexp = unitexp;
        _deffense = deffense;
        _damage = damage;
        _maxHP = maxHP;
        _Heal  = Heal;
    }
    public void ResetData()
    {
        unitName = _unitName;
        unitLevel = _unitLevel;
        unitexp = _unitexp;
        deffense = _deffense;
        damage = _damage;
        maxHP = _maxHP;
        Heal = _Heal;
    }
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
            deffense = damage * DeffenceModifier;
            Heal = maxHP * HealModifier;

            targetExp = (int)Mathf.Pow(unitLevel / 0.09f, 1.6f);
        }
    }
}