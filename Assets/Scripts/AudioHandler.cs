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

		audioManager = AudioManager.instance;
		if (!audioManager)
		{
			Debug.Log("[AudioHandler::Awake] AudioManager not found on:" + name);
			return;
		}

		switch (soundType)
		{
			case SoundType.Background:
				audioSource.loop = true;
				audioSource.playOnAwake = true;
				break;
			default:
				audioSource.loop = false;
				audioSource.playOnAwake = false;
				break;
		}

		audioManager.AddSoundSource(audioSource, soundType);
		audioSource.volume = audioManager.GetMixedVolume(soundType);
	}

	void OnDestroy()
	{
		audioManager?.RemoveSoundSource(audioSource, soundType);
	}
}

public enum SoundType
{
	Background,
	Effect,
	Interact
}
