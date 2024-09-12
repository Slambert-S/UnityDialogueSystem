using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundFXManager : MonoBehaviour
{
    public static SoundFXManager instance;
    [SerializeField] private AudioSource soundFXObject;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public void PlaySounFXClip(AudioClip audioClipm, Transform spawnTransform, float volume)
    {

        AudioSource audioSource = Instantiate(soundFXObject, spawnTransform.position, Quaternion.identity);
        audioSource.transform.SetParent(this.transform);
        audioSource.clip = audioClipm;
        audioSource.volume = volume;

        audioSource.Play();

        float clipLenght= audioSource.clip.length;

    }

    public void StopAllSoundEffect()
    {
        foreach(Transform child in this.transform)
        {
            if(child.gameObject.GetComponent<AudioSource>() != null)
            {
                child.gameObject.GetComponent<AudioSource>().Stop();
                Destroy(child.gameObject);
            }
        }
    }
}
