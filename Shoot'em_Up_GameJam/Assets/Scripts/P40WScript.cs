using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P40WScript : MonoBehaviour
{
    void Start() => SetupAnimatons();

    private void SetupAnimatons()
    {
        Animator[] anims = GetComponentsInChildren<Animator>();

        foreach (Animator a in anims)
        {
            if (a.gameObject.name == "14")
                a.SetBool("14", true);
        }
    }
}
