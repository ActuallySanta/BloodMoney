using UnityEngine;
using UnityEngine.Audio;
using System;
using Unity.Mathematics;
public class AudioManager : MonoBehaviour
{

    public Sound[] sounds;

    public static AudioManager instance;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);

        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.loop = s.doesLoop;

            if (s.doesPitchChangeRandom)
            {
                s.source.pitch = UnityEngine.Random.Range(s.minRandPitch, s.maxRandPitch);
            }
            else
            {
                s.source.pitch = s.pitch;
            }
        }
    }

    private void Start()
    {
        Play("BackgroundMusic");
    }

    public void Play(string _soundName)
    {
        Sound s = Array.Find(sounds, sound => sound.name == _soundName);

        if (s == null) return;

        Debug.Log("Played: "  + _soundName);
        s.source.Play();
    }
}
