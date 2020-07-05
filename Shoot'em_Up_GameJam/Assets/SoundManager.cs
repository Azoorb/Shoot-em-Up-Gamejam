using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField]
    AudioSource music, bulletPlayerSound, bulletEnemieSound,takeDamagePlayerSound,takeDamageEnemieSound,explosionEnemie;

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

    public void PlayBulletEnemieSound()
    {
        bulletEnemieSound.Play();
    }
    public void PlayTakeDamagePlayerSound()
    {
        takeDamagePlayerSound.Play();
    }
    public void PlayTakeDamageEnemieSound()
    {
        takeDamageEnemieSound.Play();
    }

    public void PlayExplosionEnemie()
    {
        explosionEnemie.Play();
    }
}
