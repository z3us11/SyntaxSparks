using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField] AudioClip flip;
    [SerializeField] AudioClip correct;
    [SerializeField] AudioClip wrong;
    [SerializeField] AudioClip win;
    
    AudioSource audioSource;

    public static SoundManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        audioSource = GetComponent<AudioSource>();
    }
    public void PlayFlipSound()
    {
        Debug.Log("Play Flip Sound");
        audioSource.PlayOneShot(flip);
    }

    public void PlayCorrectSound()
    {
        audioSource.PlayOneShot(correct);
    }

    public void PlayWrongSound()
    {
        audioSource.PlayOneShot(wrong);
    }

    public void PlayWinSound()
    {
        audioSource.PlayOneShot(win);
    }

}
