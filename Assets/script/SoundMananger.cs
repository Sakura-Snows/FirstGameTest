using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundMananger : MonoBehaviour
{
    public static SoundMananger sound_class;
    public AudioSource audioSource;
    [SerializeField]private AudioClip jumpAudio,hurtAudio,collectionAudio;

    private void Awake()
    {
        sound_class = this;   
    }
    public void JumpAudio()
    {
        audioSource.clip = jumpAudio;
        audioSource.Play();
    }
    public void HurtAudio()
    {
        audioSource.clip = hurtAudio;
        audioSource.Play();
    }
    public void CollectionAudio()
    {
        audioSource.clip = collectionAudio;
        audioSource.Play();
    }
}
