using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LosePanel : MonoBehaviour
{
    [SerializeField] private Button backBTN;
    [SerializeField] private Button retryBTN;

    private void Start()
    {
        backBTN.onClick.AddListener(() => SceneManager.LoadScene("Hub"));
        retryBTN.onClick.AddListener(() => SceneManager.LoadScene(SceneManager.GetActiveScene().name));
    }
}
