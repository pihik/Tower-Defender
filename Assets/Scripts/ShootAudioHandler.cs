using UnityEngine;

[RequireComponent(typeof(TargetLocator))]
public class ShootAudioHandler : AudioHandler
{
    TargetLocator targetLocator;

    ShootAudioHandler()
    {
        soundType = SoundType.Effect;
    }

    protected override void Awake()
    {
        base.Awake();

        targetLocator = GetComponent<TargetLocator>();

        if (!targetLocator)
        {
            Debug.Log("[ShootAudioHandler::Awake] Target locator not found on: " + name);
            return;
        }

        targetLocator.OnShoot += PlayShootSound;
    }

    void PlayShootSound()
    {
        audioSource.Play();
    }

    void OnDisable()
    {
        targetLocator.OnShoot -= PlayShootSound;
    }
}
