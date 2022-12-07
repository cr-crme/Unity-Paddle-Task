using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class that manage the audio of the scene
/// It plays the music or the ambiant sound according to the menu selection
/// Choses song according to menu selection
/// Set audio volume according to menu selection
/// </summary>
public class MusicManager : MonoBehaviour
{
    //The selected music clip from the menu
    private AudioClip musicClip;

    //The index of the selected clip
    private int musicClipIndex;

    //List of the audio sources 
    private AudioSource[] audioSources;

    //Menu selection of music on/off
    private bool toggleMusicOn;

    //Menu selection of ambiant sound on/off
    private bool toggleAmbiantOn;

    //Audio volume from menu
    private float volume;


    void Start()
    {
        audioSources = GetComponentsInChildren<AudioSource>();

      
        toggleMusicOn = (GlobalPreferences.Instance.sceneMusicIndex >= 0) ;
       
        toggleAmbiantOn = GlobalPreferences.Instance.toggleAmbiantSoundOn;
        volume = GlobalPreferences.Instance.StartingMusicVolume/10.0f;

        for (int i = 0; i < audioSources.Length; i++)
        {
            //Music audio source
            if (audioSources[i].name == "AmbiantMusic")
            {
                if (toggleMusicOn != false)
                {
                    musicClipIndex = GlobalPreferences.Instance.sceneMusicIndex;
                   
                    if (musicClipIndex >= 0)
                    {
                        musicClip = GlobalPreferences.Instance.SceneMusic[musicClipIndex];
                        audioSources[i].enabled = true;
                        audioSources[i].clip = musicClip;
                        audioSources[i].volume = volume;
                        audioSources[i].Play();
                    }
                    else 
                    {
                        audioSources[i].enabled = false;
                    }
                }
                else
                {
                    audioSources[i].enabled = false;
                }
            }

            //Ambiant sounds audio source
            else
            {
                if (toggleAmbiantOn != false)
                {
                  
                    audioSources[i].enabled = true;
                    audioSources[i].volume = volume;
                    audioSources[i].Play();
                }
                else
                {
                    audioSources[i].enabled = false;
                }
            }
        }
    }
}
