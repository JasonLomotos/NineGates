using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class SettingsMenu : MonoBehaviour
{
    [Header("Menu Containers")]
    public GameObject settingsButtonsContainer; // Parent of Graphics, Game, Audio, Back buttons
    public GameObject graphicsPanel;
    public GameObject gamePanel;
    public GameObject audioPanel;

    [Header("Graphics Settings")]
    public TMP_Dropdown resolutionDropdown;

    [Header("Game Settings")]
    public Slider sensitivitySlider;
    public TextMeshProUGUI sensitivityValueText; // Optional: To display the value

    [Header("Audio Settings")]
    public Slider masterVolumeSlider;
    public TextMeshProUGUI masterVolumeValueText; // Optional: To display the value

    private Resolution[] resolutions;

    void Start()
    {
        // Deactivate all panels to ensure a clean start
        graphicsPanel.SetActive(false);
        gamePanel.SetActive(false);
        audioPanel.SetActive(false);

        // --- Populate Graphics Settings ---
        resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();
        
        List<string> options = new List<string>();
        int currentResolutionIndex = 0;
        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            if (!options.Contains(option))
            {
                options.Add(option);
            }

            if (resolutions[i].width == Screen.currentResolution.width &&
                resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = options.Count - 1;
            }
        }
        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();

        // --- Load Saved Settings ---
        float sensitivity = PlayerPrefs.GetFloat("CameraSensitivity", 5f);
        sensitivitySlider.value = sensitivity;
        UpdateSensitivityText(sensitivity);

        float volume = PlayerPrefs.GetFloat("MasterVolume", 1f);
        masterVolumeSlider.value = volume;
        AudioListener.volume = volume;
        UpdateMasterVolumeText(volume);
    }
    
    // --- Navigation ---
    public void ShowGraphicsMenu()
    {
        settingsButtonsContainer.SetActive(false);
        graphicsPanel.SetActive(true);
    }

    public void ShowGameMenu()
    {
        settingsButtonsContainer.SetActive(false);
        gamePanel.SetActive(true);
    }

    public void ShowAudioMenu()
    {
        settingsButtonsContainer.SetActive(false);
        audioPanel.SetActive(true);
    }

    public void ReturnToSettingsMenu()
    {
        settingsButtonsContainer.SetActive(true);
        graphicsPanel.SetActive(false);
        gamePanel.SetActive(false);
        audioPanel.SetActive(false);
    }

    // --- Settings Functions ---
    public void SetResolution(int resolutionIndex)
    {
        // Note: This simple method might not be perfect if you filter resolutions.
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    public void SetSensitivity(float sensitivity)
    {
        PlayerPrefs.SetFloat("CameraSensitivity", sensitivity);
        UpdateSensitivityText(sensitivity);
    }

    private void UpdateSensitivityText(float value)
    {
        if (sensitivityValueText != null)
            sensitivityValueText.text = value.ToString("F2");
    }

    public void SetMasterVolume(float volume)
    {
        AudioListener.volume = volume;
        PlayerPrefs.SetFloat("MasterVolume", volume);
        UpdateMasterVolumeText(volume);
    }

    private void UpdateMasterVolumeText(float value)
    {
        if (masterVolumeValueText != null)
            masterVolumeValueText.text = Mathf.RoundToInt(value * 100).ToString();
    }
}
