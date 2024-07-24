using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFXManager : MonoBehaviour
{
    public static VFXManager instance;

    public GameObject VFX_Deffense;
    public GameObject VFX_Puch;
   

    private void Awake()
    {
        instance = this;
    }

    public void EffetDef(Vector2 pos)
    {
        SpwanObject(VFX_Deffense, 1f, pos + new Vector2(0,1.5f));
    }

    public void EffetPuch(Vector2 pos)
    {
        SpwanObject(VFX_Puch, 1f, pos + new Vector2(0, 1.5f));
    }


    private void SpwanObject(GameObject Obj, float TimeDestroy, Vector2 pos)
    {
        GameObject go = Instantiate(Obj, pos,Quaternion.identity);
        Destroy(go, TimeDestroy);
    }
}
