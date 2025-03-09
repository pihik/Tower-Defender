using UnityEngine;
using UnityEngine.UI;

public class AudioSettings : MonoBehaviour
{
    [SerializeField] Slider masterVolumeSlider;
    [SerializeField] Slider backgroundVolumeSlider;
    [SerializeField] Slider effectVolumeSlider;
    [SerializeField] Slider interactSlider;

    void Start()
    {
        AudioManager audioManager = AudioManager.instance;
        if (!audioManager)
        {
            Debug.Log("[AudioSettings::Start] AudioManager not found.");
            return;
        }

        masterVolumeSlider.value = audioManager.GetMasterVolume();
        backgroundVolumeSlider.value = audioManager.GetVolume(SoundType.Background);
        effectVolumeSlider.value = audioManager.GetVolume(SoundType.Effect);
        interactSlider.value = audioManager.GetVolume(SoundType.Interact);
    }

    public void Apply()
    {
        AudioManager audioManager = AudioManager.instance;
        if (!audioManager)
        {
            Debug.Log("[AudioSettings::Apply] AudioManager not found.");
            return;
        }

        audioManager.SetMasterVolume(masterVolumeSlider.value);
        audioManager.SetVolume(SoundType.Background, backgroundVolumeSlider.value);
        audioManager.SetVolume(SoundType.Effect, effectVolumeSlider.value);
        audioManager.SetVolume(SoundType.Interact, interactSlider.value);
    }
}
