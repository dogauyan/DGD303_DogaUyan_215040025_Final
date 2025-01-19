using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Audio;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UI;


[System.Serializable]
public class AssetReferenceAudioMixer : AssetReferenceT<AudioMixer> {
    public AssetReferenceAudioMixer(string guid) : base(guid) { }
}
[System.Serializable]
public class AssetReferenceAudioClip : AssetReferenceT<AudioClip> {
    public AssetReferenceAudioClip(string guid) : base(guid) { }
}

public class VolumeManager : MonoBehaviour {
    [SerializeField] private AssetReferenceAudioMixer MainMixerReference;
    public static AudioMixer MainMixer = null;
    public static AudioMixerGroup Master;
    public static AudioMixerGroup SoundFx;
    public static AudioMixerGroup Music;
    public enum AudioMixerGroupType
    {
        Master,
        SoundFx,
        Music
    }

    [SerializeField] private Slider MasterSlider;
    [SerializeField] private Slider SoundFxSlider;
    [SerializeField] private Slider MusicSlider;

    private void OnEnable() {
        if (MainMixer != null) {
            if (MasterSlider) {
                MasterSlider.value = PlayerPrefs.GetFloat(master_mixertag, 0.5f);
                MasterVolume(MasterSlider.value);
            }
            if (SoundFxSlider) {
                SoundFxSlider.value = PlayerPrefs.GetFloat(soundfx_mixertag, 0.5f);
                SoundFxVolume(SoundFxSlider.value);
            }
            if (MusicSlider) {
                MusicSlider.value = PlayerPrefs.GetFloat(music_mixertag, 0.5f);
                MusicVolume(MusicSlider.value);
            }

            return;
        }

        MainMixerReference.LoadAssetAsync().Completed += (asyncOperationHandle) => {
            if (asyncOperationHandle.Status == AsyncOperationStatus.Succeeded) {
                MainMixer = Instantiate(asyncOperationHandle.Result);
                Master = MainMixer.FindMatchingGroups("Master")[0];
                SoundFx = MainMixer.FindMatchingGroups("SoundFX")[0];
                Music = MainMixer.FindMatchingGroups("Music")[0];

                MasterVolume(PlayerPrefs.GetFloat(master_mixertag, 0.5f));
                SoundFxVolume(PlayerPrefs.GetFloat(soundfx_mixertag, 0.5f));
                MusicVolume(PlayerPrefs.GetFloat(music_mixertag, 0.5f));

                if (MasterSlider) MasterSlider.value = PlayerPrefs.GetFloat(master_mixertag, 0.5f);
                if (SoundFxSlider) SoundFxSlider.value = PlayerPrefs.GetFloat(soundfx_mixertag, 0.5f);
                if (MusicSlider) MusicSlider.value = PlayerPrefs.GetFloat(music_mixertag, 0.5f);

                //Debug.Log("Loaded.. the mixer");
            } else {
                Debug.Log("Failed to load!.. the mixer");
            }
        };
    }

    private const string master_mixertag = "MasterVolume";
    private const string soundfx_mixertag = "SoundFxVolume";
    private const string music_mixertag = "MusicVolume";
    public void MasterVolume(float volume) {
        Master.audioMixer.SetFloat(master_mixertag, Mathf.Log10(volume) * 20f);
        PlayerPrefs.SetFloat(master_mixertag, volume);
        PlayerPrefs.Save();
    }
    public void SoundFxVolume(float volume) {
        SoundFx.audioMixer.SetFloat(soundfx_mixertag, Mathf.Log10(volume) * 20f);
        PlayerPrefs.SetFloat(soundfx_mixertag, volume);
        PlayerPrefs.Save();
    }
    public void MusicVolume(float volume) {
        Music.audioMixer.SetFloat(music_mixertag, Mathf.Log10(volume) * 20f);
        PlayerPrefs.SetFloat(music_mixertag, volume);
        PlayerPrefs.Save();
    }
}
