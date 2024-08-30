using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;
    [SerializeField] private AudioSource soundFXObject;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        
    }

    public void PlaySoundFX(Transform transform, AudioClip[] clip)
    {
        int rand = Random.Range(0,clip.Length);
        AudioSource audioSource = Instantiate(soundFXObject, transform.position, Quaternion.identity);
        audioSource.volume = (0.5f);
        audioSource.clip = clip[rand];
        audioSource.Play();

        Destroy (audioSource.gameObject, audioSource.clip.length);

    }
}
