using System;
using UnityEngine;

[System.Serializable]
public class Sound
{
    public string name;
    public AudioClip clip;
    [Range(0f, 3f)]
    public float volume = 0.3f;
    [Range(-3f, 3f)]
    public float pitch = 1f;
    public bool loop;
    public bool instantPlay;
    public bool isMusic;

    [HideInInspector]
    public AudioSource audioSource;
}

public class AudioPlayer : MonoBehaviour
{
    public static AudioPlayer instance;

    [SerializeField] private Sound[] sounds;

    private void Awake()
    {
        if (!instance)
            instance = this;
        else
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);

        foreach (Sound sound in sounds)
        {
            sound.audioSource = AddAudioSource(sound);
            if (sound.instantPlay)
                sound.audioSource.Play();
        }
    }

    private void OnEnable()
    {
        foreach (var sound in sounds)
        {
            if (sound.instantPlay)
            {
                sound.audioSource.Play();
            }
        }
    }

    public void PlayAudio(string name)
    {
        Sound sound = Array.Find(sounds, x => x.name == name);
        if (sound == null)
        {
            Debug.Log("Nie znaleziono audio o nazwie: " + name);
            return;
        }
        sound.audioSource.Play();
    }

    private AudioSource AddAudioSource(Sound sound)
    {
        AudioSource audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = sound.clip;
        audioSource.loop = sound.loop;
        audioSource.volume = sound.volume;
        audioSource.pitch = sound.pitch;

        return audioSource;
    }
}
