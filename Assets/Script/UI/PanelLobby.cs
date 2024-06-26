using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PanelLobby : MonoBehaviour
{
    [SerializeField] private TMP_Text akunLevelText;
    //[SerializeField] private Slider akunExpSlider;
    [SerializeField] private TMP_Text akunExpText;
    [SerializeField] private TMP_Text akunMoneyText;
    [SerializeField] private TMP_Text akunStaminaText;

    private void Start()
    {
        Akun akun = Funcs.GetAkun();
        akunLevelText.text = akun.akunLvl.ToString();
        akunExpText.text = akun.akunExp.ToString();
        //akunMoneyText.text = "$ " + akun.akunMoney.ToString();
        akunStaminaText.text = akun.akunStamina.ToString();
    }

}
