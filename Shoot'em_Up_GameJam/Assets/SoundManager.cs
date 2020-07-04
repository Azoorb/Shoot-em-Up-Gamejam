using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField]
    AudioSource music, bulletPlayerSound;

    public static SoundManager instance;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }

    public void PlayMusic()
    {
        music.Play();
    }

    public void PlayBulletPlayerSound()
    {
        bulletPlayerSound.Play();
    }
}
