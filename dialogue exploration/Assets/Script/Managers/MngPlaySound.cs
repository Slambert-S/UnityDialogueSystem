using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

//using System.Diagnostics;
using UnityEngine;

public class MngPlaySound : MonoBehaviour
{
    [SerializeField]
    private AudioSource backgroundMusicAudioSource;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void HandlePlaySounds(dialogueObject currentLine)
    {
        //Check if bakground Music need to be change
        if (currentLine.backgroundMusic != null)
        {
            //Change the background music
            ChangeBackgroundMusic(currentLine.backgroundMusic);
        }


        // Check if Line specific sound effect need to be played
        if (currentLine.dialogueSoundEffect != null)
        {
            // PLay that sound effect
            PlaySoundEffect(currentLine.dialogueSoundEffect);
        }

        if(currentLine.playActorSoundEffect == false)
        {
            return;
        }
        // Check all actor for sound effect to play 
        foreach(ActorList actor in currentLine.actorList)
        {
            if(actor.actorSoundEffect > -1)
            {
                if(actor.actorSoundEffect < actor.character.audioClipList.Count)
                {
                    // PLay their sound effect
                    PlaySoundEffect(actor.character.audioClipList[actor.actorSoundEffect]);
                }
                else
                {
                    Debug.Log("Sound effect index out of range");
                }
            }
        }

    }

    public void StopAllSoundEffect()
    {
        SoundFXManager.instance.StopAllSoundEffect();
    }

    private void ChangeBackgroundMusic(AudioClip backgroundMusic)
    {
        backgroundMusicAudioSource.clip = backgroundMusic;
        backgroundMusicAudioSource.Play();
    }

    private void PlaySoundEffect(AudioClip audioClip)
    {
        Debug.Log("In play sound effect");
        SoundFXManager.instance.PlaySounFXClip(audioClip , this.transform, 1f) ;
    }


}