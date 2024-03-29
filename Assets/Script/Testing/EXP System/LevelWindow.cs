using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelWindow : MonoBehaviour
{
    private Text levelText;
    private Image experienceBarImage;
    private LevelSystem levelSystem;

    private void Awake()
    {
        levelText = transform.Find("levelText").GetComponent<Text>();
        experienceBarImage = transform.Find("experienceBar").Find("bar").GetComponent<Image>();

        transform.Find("experience5Btn").GetComponent<Button>().onClick.AddListener(() => levelSystem.AddExperience(5));
        transform.Find("experience50Btn").GetComponent<Button>().onClick.AddListener(() => levelSystem.AddExperience(50));
        transform.Find("experience500Btn").GetComponent<Button>().onClick.AddListener(() => levelSystem.AddExperience(500));
    }

    private void SetExperienceBarSize(float ExperienceNormalized)
    {
        experienceBarImage.fillAmount = ExperienceNormalized;
    }

    private void SetLevelNumber(int levelNumber)
    {
        levelText.text = "LEVEL\n" + (levelNumber + 1);
    }

    public void SetLevelSystem(LevelSystem levelSystem)
    {
        // Set the LevelSystem Object
        this.levelSystem = levelSystem;

        // Update the starting values
        SetLevelNumber(levelSystem.GetLevelNumber());
        SetExperienceBarSize(levelSystem.GetExperienceNormalized());

        // Subscribe to the changed events
        levelSystem.OnExperienceChanged += LevelSystem_OnExperienceChanged;
        levelSystem.OnLevelChanged += LevelSystem_OnLevelChanged;
    }

    private void LevelSystem_OnLevelChanged(object sender, System.EventArgs e)
    {
        // level changed, update text
        SetLevelNumber(levelSystem.GetLevelNumber());
    }

    private void LevelSystem_OnExperienceChanged(object sender, System.EventArgs e)
    {
        // experience changed, update bar size
        SetExperienceBarSize(levelSystem.GetExperienceNormalized());
    }
}
