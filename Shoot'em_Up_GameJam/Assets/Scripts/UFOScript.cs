using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

public class UFOScript : MonoBehaviour
{
    void Start()
    {
        //Animation Setup
        Animator[] animators = GetComponentsInChildren<Animator>();

        Debug.Log(animators.Length);

        foreach (Animator a in animators)
        {
            if (a.gameObject.name == "Left")
                a.SetBool("Left", true);
            else
                Debug.Log(a.gameObject.name);
        }
    }
}
