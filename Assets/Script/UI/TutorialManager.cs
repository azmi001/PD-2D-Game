using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{
    public Image tutorialImage;
    public Button nextButton;
    public Button prevButton;
    public Sprite[] tutorialSprites; // Array untuk menyimpan sprite tata cara bermain
    private int currentIndex = 0;

    void Start()
    {
        // Pastikan halaman pertama ditampilkan saat menu dibuka
        UpdateTutorialPage();
    }

    public void NextPage()
    {
        if (currentIndex < tutorialSprites.Length - 1)
        {
            currentIndex++;
            UpdateTutorialPage();
        }
    }

    public void PrevPage()
    {
        if (currentIndex > 0)
        {
            currentIndex--;
            UpdateTutorialPage();
        }
    }

    private void UpdateTutorialPage()
    {
        tutorialImage.sprite = tutorialSprites[currentIndex];
        prevButton.interactable = currentIndex > 0;
        nextButton.interactable = currentIndex < tutorialSprites.Length - 1;
    }
}
