using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    #region Singleton
    public static AudioManager instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
			enabled = false;
			return;
		}
    }
    #endregion

    float masterVolume = 0.5f;
    float backgroundVolume = 0.5f;
    float effectVolume = 0.5f;
    float interactVolume = 0.5f;

    AudioSource backgroundSource; 
    HashSet<AudioSource> effectSources = new HashSet<AudioSource>();
	HashSet<AudioSource> interactSources = new HashSet<AudioSource>();


	[SerializeField] AudioSource effectSource;
    [SerializeField] AudioSource interactSource;

    public void AddSoundSource(AudioSource audioSource, SoundType soundType)
    {
        switch (soundType)
        {
            case SoundType.Background:
                backgroundSource = audioSource;
                break;
            case SoundType.Effect:
                effectSources.Add(audioSource);
                break;
            case SoundType.Interact:
                interactSources.Add(audioSource);
                break;
        }
    }

    void UpdateVolume()
    {
        if (backgroundSource)
        {
			backgroundSource.volume = masterVolume * backgroundVolume;
		}

        foreach (AudioSource audioSource in effectSources)
        {
            audioSource.volume = masterVolume * effectVolume;
        }

        foreach (AudioSource audioSource in interactSources)
        {
            audioSource.volume = masterVolume * interactVolume;
        }
    }

    public void PlayEffectSFX(AudioClip clip)
    {
        effectSource.PlayOneShot(clip);
    }

    public void PlayInteractSFX(AudioClip clip)
    {
        interactSource.PlayOneShot(clip);
    }

    public void SetMasterVolume(float volume)
    {
        masterVolume = volume;
        UpdateVolume();
    }

    public void SetVolume(SoundType soundType, float volume)
    {
        switch (soundType)
        {
            case SoundType.Background:
                backgroundVolume = volume;
                break;
            case SoundType.Effect:
                effectVolume = volume;
                break;
            case SoundType.Interact:
                interactVolume = volume;
                break;
        }

        UpdateVolume();
    }

    public float GetMasterVolume()
    {
        return masterVolume;
    }

    public float GetVolume(SoundType soundType)
    {
        switch (soundType)
        {
            case SoundType.Background:
                return backgroundVolume;
            case SoundType.Effect:
                return effectVolume;
            case SoundType.Interact:
                return interactVolume;
        }

        return 0;
    }

    public float GetMixedVolume(SoundType soundType)
    {
        switch (soundType)
        {
            case SoundType.Background:
                return masterVolume * backgroundVolume;
            case SoundType.Effect:
                return masterVolume * effectVolume;
            case SoundType.Interact:
                return masterVolume * interactVolume;
        }
        return 0;
    }

    public void RemoveSoundSource(AudioSource audioSource, SoundType soundType)
    {
        switch (soundType)
        {
            case SoundType.Background:
                backgroundSource = null;
                break;
            case SoundType.Effect:
                effectSources.Remove(audioSource);
                break;
            case SoundType.Interact:
                interactSources.Remove(audioSource);
                break;
        }
    }
}