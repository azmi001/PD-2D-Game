using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetGameData : MonoBehaviour
{
    public void ResetData()
    {
        PlayerPrefs.DeleteAll();
    }
}
