using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class loadPrefs : MonoBehaviour
{
    [Header("General String")]
    [SerializeField] private bool canUse = false;
    [SerializeField] private menuController MenuController;

    [Header("Volume Settings")]
    [SerializeField] private TMP_Text volumeTextValue = null;
    [SerializeField] private Slider volumeSlider = null;

    [Header("Brightness Settings")]
    [SerializeField] private Slider brightnesSlider = null;
    [SerializeField] private TextMeshProUGUI brightnessValue = null;

    [Header("Quality Settings")]
    [SerializeField] private TMP_Dropdown qualityDropdown;

    [Header("Fullscreen Settings")]
    [SerializeField] private Toggle fullscreenToggle;

    private void Awake()
    {
        if (canUse)
        {
            if (PlayerPrefs.HasKey("masterVolume"))
            {
                float localVolume = PlayerPrefs.GetFloat("masterVolume");

                volumeTextValue.text = localVolume.ToString("0.0");
                volumeSlider.value = localVolume;
                AudioListener.volume = localVolume;
            }
            else
            {
                MenuController.ResetBtn("Audio");
            }

            if (PlayerPrefs.HasKey("masterQuality"))
            {
                int localQuality = PlayerPrefs.GetInt("masterQuality");
                qualityDropdown.value = localQuality;
                QualitySettings.SetQualityLevel(localQuality);
            }

            if (PlayerPrefs.HasKey("masterFullscreen"))
            {
                int localFullscreen = PlayerPrefs.GetInt("masterFullscreen");
                if (localFullscreen == 1)
                {
                    Screen.fullScreen = true;
                    fullscreenToggle.isOn = true;
                }
                else
                {
                    Screen.fullScreen = true;
                    fullscreenToggle.isOn = true;
                }
            }

            if (PlayerPrefs.HasKey("masterBrightness"))
            {
                float localBrightness = PlayerPrefs.GetFloat("masterBrightness");

                brightnessValue.text = localBrightness.ToString("0.0");
                brightnesSlider.value = localBrightness;

                
            }
        }
    }
}
