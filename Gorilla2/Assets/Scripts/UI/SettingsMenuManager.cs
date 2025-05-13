using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace UI
{
    public static class SettingsKeys
    {
        public const string General = "General";
        public const string Music = "Music";
        public const string Sfx = "SFX";
        public const string ResolutionIndex = "ResolutionIndex";
        public const string FullScreen = "FullScreen";
    }

    public class SettingsMenuManager : MonoBehaviour
    {
        [Header("Audio")]
        [SerializeField] private AudioMixer audioMixer;
        [SerializeField] private Slider sliderGeneral;
        [SerializeField] private Slider sliderMusic;
        [SerializeField] private Slider sliderSfx;
    
        [Header("Resolution")]
        [SerializeField] private TMP_Dropdown dropdownResolution;
    
        [Header("Toggle")]
        [SerializeField] private Toggle toggleFullScreen;
    
        private Resolution[] resolutions;
    
        private void Start()
        {
            if (dropdownResolution is not null)
            {
                dropdownResolution.ClearOptions();
                AddResolutionOnDropdown(dropdownResolution);
                dropdownResolution.value = PlayerPrefs.HasKey(SettingsKeys.ResolutionIndex) ? PlayerPrefs.GetInt(SettingsKeys.ResolutionIndex) : dropdownResolution.options.Count - 1;
            }

            if (PlayerPrefs.HasKey(SettingsKeys.FullScreen))
            {
                Screen.fullScreen = PlayerPrefs.GetInt(SettingsKeys.FullScreen) == 1;
            }
        
            if (toggleFullScreen is not null)
            {
                toggleFullScreen.isOn = Screen.fullScreen;
            }
        }
        private void OnEnable()
        {
            InitAudioSettings(sliderGeneral, SettingsKeys.General);
            InitAudioSettings(sliderMusic, SettingsKeys.Music);
            InitAudioSettings(sliderSfx, SettingsKeys.Sfx);
        }

        public void SetFullScreen(bool isFullScreen)
        {
            Screen.fullScreen = isFullScreen;
            PlayerPrefs.SetInt(SettingsKeys.FullScreen, isFullScreen ? 1 : 0);
            PlayerPrefs.Save();
        }

        #region Audio
    
        public void SetVolumeGeneral(float volume)
        {
            SetVolume(SettingsKeys.General, volume);
        }
    
        public void SetVolumeSfx(float volume)
        {
            SetVolume(SettingsKeys.Sfx, volume);
        }
    
        public void SetVolumeMusic(float volume)
        {
            SetVolume(SettingsKeys.Music, volume);
        }
    
        private void SetVolume(string key, float volume)
        {
            if (audioMixer is null)
            {
                Debug.LogWarning("AudioMixer is not assigned in the inspector.");
                return;
            }
        
            if (string.IsNullOrEmpty(key))
            {
                Debug.LogWarning("Audio key is null or empty.");
                return;
            }
        
            if (volume is < -80f or > 20f)
            {
                Debug.LogWarning($"Volume value {volume} is out of range. It must be between -80 and 20.");
                return;
            }

            audioMixer.SetFloat(key, volume);
            PlayerPrefs.SetFloat(key, volume);
            PlayerPrefs.Save();
        }
    
        private static void InitAudioSettings(Slider slider, string key)
        {
            if (slider is not null && PlayerPrefs.HasKey(key))
            {
                slider.value = PlayerPrefs.GetFloat(key);
            }
        }
    
        #endregion
    

        #region Resolution
    
        private void AddResolutionOnDropdown(TMP_Dropdown dropdown)
        {
            if (resolutions is null || resolutions.Length == 0)
            {
                resolutions = Screen.resolutions.GroupBy(r => new { r.width, r.height }).Select(group => group.First()).ToArray();
            }
        
            List<string> options = resolutions.Select(resolution => resolution.width + "x" + resolution.height).ToList();
        
            dropdown.AddOptions(options);
        }
    
        public void SetResolution(int resolutionIndex)
        {
            if (resolutionIndex < 0 || resolutionIndex >= resolutions.Length)
            {
                return;
            }
        
            Resolution resolution = resolutions[resolutionIndex];
            Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
        
            PlayerPrefs.SetInt(SettingsKeys.ResolutionIndex, resolutionIndex);
            PlayerPrefs.Save();
        }
    
        #endregion
    }
}