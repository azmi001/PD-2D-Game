using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.Audio;
using UnityEngine;
using Unity.VisualScripting;


public class AudioManager : MonoBehaviour
{
    public static AudioManager instance; // Singleton instance

    public Sound[] sounds;

    private void Awake()
    {
        // Ensure only one instance of AudioManager exists
        if (instance == null)
        {
            instance = this;
            foreach (Sound s in sounds)
            {
                s.source = gameObject.AddComponent<AudioSource>();
                s.source.clip = s.clip;
                s.source.volume = s.volume;
                s.source.pitch = s.pitch;
                s.source.loop = s.loop;
                s.source.outputAudioMixerGroup = s.am;
            }
        }
        else
        {
            // If an instance already exists, destroy this one
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    public void Play(string name)
    {
        Sound s = Array.Find(sounds, sounds => sounds.name == name);
        s.source.Play();
    }

    public void Stop(string name)
    {
        Sound s = Array.Find(sounds, sounds => sounds.name == name);
        s.source.Stop();
    }

    public void Pause(string name)
    {
        Sound s = Array.Find(sounds, sounds => sounds.name == name);
        s.source.Pause();
    }

    public void SetVolume(string name, float volume)
    {
        Sound s = Array.Find(sounds, sounds => sounds.name == name);
        s.source.volume = volume;
    }

    public bool IsPlaying(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s != null)
        {
            return s.source.isPlaying;
        }
        return false;
    }

    public void StopAll()
    {
        foreach (var item in sounds)
        {
            item.source.Stop();
        }
    }

    /*public void PlaySound(int clipIndex)
    {
        audioSource.PlayOneShot(clips[clipIndex]);
    }*/
}