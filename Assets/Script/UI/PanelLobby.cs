using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class PanelLobby : MonoBehaviour
{
    [SerializeField] private TMP_Text akunLevelText;
    //[SerializeField] private Slider akunExpSlider;
    [SerializeField] private TMP_Text akunExpText;
    [SerializeField] private TMP_Text akunMoneyText;
    [SerializeField] private TMP_Text akunStaminaText;
    [SerializeField] private TMP_Text countdownStaminaText;

    private void Start()
    {
        Akun akun = Funcs.GetAkun();
        akunLevelText.text = akun.akunLvl.ToString();
        akunExpText.text = akun.akunExp.ToString();
        //akunMoneyText.text = "$ " + akun.akunMoney.ToString();
        akunStaminaText.text = $"{akun.akunStamina} / 100";
    }
    private void Update()
    {
        TimeSpan remainingTime = Funcs.GetCountdownStamina() - DateTime.Now;
        if (remainingTime.TotalSeconds > 0)
        {
            string countdownString = string.Format("{0:D2}:{1:D2}:{2:D2}",
                                                remainingTime.Hours,
                                                remainingTime.Minutes,
                                                remainingTime.Seconds);
            countdownStaminaText.text = countdownString; // Tampilkan di Text UI
        }
        else
        {
            countdownStaminaText.text = $"CLAIM";
        }
    }
    public void ClaimStamina()
    {
        Actions.ClaimStamina?.Invoke(() =>
        {
            akunStaminaText.text = $"{Funcs.GetAkun().akunStamina} / 100";

        });
    }
}
