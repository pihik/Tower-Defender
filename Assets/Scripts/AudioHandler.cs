using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioHandler : MonoBehaviour
{
    [SerializeField] protected SoundType soundType;

    protected AudioSource audioSource;
    protected AudioManager audioManager;

    virtual protected void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        if (!audioSource)
        {
            Debug.Log("[AudioHandler::Awake] AudioSource not found.");
            return;
        }

        audioManager = AudioManager.instance;
        if (!audioManager)
        {
            Debug.Log("[AudioHandler::Awake] AudioManager not found on:" + name);
            return;
        }

        if (soundType == SoundType.Background)
        {
            audioSource.loop = true;
            audioSource.playOnAwake = true;
        }
        else
        {
            audioSource.loop = false;
            audioSource.playOnAwake = false;
        }

        audioManager.AddSoundSource(audioSource, soundType);
        audioSource.volume = audioManager.GetMixedVolume(soundType);
    }

    void OnDestroy()
    {
        if (audioManager && audioSource)
        {
            audioManager.RemoveSoundSource(audioSource, soundType);
        }
    }
}

public enum SoundType
{
    Background,
    Effect,
    Interact
}
