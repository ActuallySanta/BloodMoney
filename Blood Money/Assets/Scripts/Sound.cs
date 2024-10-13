using UnityEngine;
using UnityEngine.Audio;

[System.Serializable]
public class Sound
{
    public string name;
    public AudioClip clip;

    [HideInInspector] public AudioSource source;
    
    public bool doesLoop;

    [Range(0, 1)]
    public float volume;

    [Range(.1f, 3)]
    public float pitch = 1f;
}
