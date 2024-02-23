using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleHUD : MonoBehaviour
{
    public Text nameText;
    public Text levelText;
    public Slider hpSlider;

    public void SetHUD(Unit unit)
    {
        nameText.text = unit.character.unitName;
        levelText.text = "Lvl " + unit.currentLv;
        hpSlider.maxValue = unit.character.maxHP;
        hpSlider.value = unit.currentHP;
    }

    public void SetHP(int hp)
    {
        hpSlider.value = hp;
    }
}
