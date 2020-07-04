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
        music.volume = 0.2f;
    }

    public void PlayBulletPlayerSound()
    {
        bulletPlayerSound.volume = 0.2f;
        bulletPlayerSound.Play();
    }
}
