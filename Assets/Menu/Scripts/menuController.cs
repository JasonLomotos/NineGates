using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class menuController : MonoBehaviour
{
    [Header("Volume")]
    [SerializeField] private TextMeshProUGUI volumeValueText = null;
    [SerializeField] private Slider volumeSlider = null;
    [SerializeField] private float defaultVolume = 0.0f;

    [Header("Level to load")]
    public string loadState1;
    private string toLoad;
    [SerializeField] private GameObject noSaveState = null;

    [Header("Graphics")]
    [SerializeField] private Slider brightnesSlider = null;
    [SerializeField] private TextMeshProUGUI brightnessValue = null;
    [SerializeField] private float defaultBrightness = 1;
    
    [Space(10)]
    [SerializeField] private TMP_Dropdown qualityDropdown;
    [SerializeField] private Toggle fullscreenToggle;

    [Header("Resolution Dropdown")]
    public TMP_Dropdown resolutionDropdown;
    private Resolution[] resolutions;

    private void Start()
    {
        resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();

        List<string> options = new List<string>();

        int curentResolutionIndex = 0;

        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);

            if (resolutions[i].width == Screen.width && resolutions[i].height == Screen.height)
            {
                curentResolutionIndex = i;
            }

        }

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = curentResolutionIndex;
        resolutionDropdown.RefreshShownValue();    
    }

    public void setResolution(int resolutonIndex)
    {
        Resolution resolution = resolutions[resolutonIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    private int _qualityLvl;
    private bool _fullscreen;
    private float _brightnessLvl;

    //LEVEL TO LOAD
    public void newGameDialogueYes()
    {
        SceneManager.LoadScene(loadState1);
        
    }

    public void loadGameDialogueYes()
    {
        if (PlayerPrefs.HasKey("savedState"))
        {
            toLoad = PlayerPrefs.GetString("savedState");
            SceneManager.LoadScene(toLoad);
        }
        else
        {
            noSaveState.SetActive(true);
        }

    }

    public void exitBtn()
    {
        Application.Quit();
    }

    //VOLUME
    public void setVolume(float volume)
    {
        AudioListener.volume = volume;
        volumeValueText.text = volume.ToString("0.0");
    }

    public void volumeApply()
    {
        PlayerPrefs.SetFloat("masterVolume", AudioListener.volume);
    }

    // Graphics

    public void setBrightness(float brightness)
    {
        _brightnessLvl = brightness;
        brightnessValue.text = brightness.ToString("0.0");
    }

    public void setFullScreen(bool isFullScreen)
    {
        _fullscreen = isFullScreen;
    }

    public void setQuality(int qualityIndex)
    {
        _qualityLvl =  qualityIndex;
    }

    public void graphicsApply()
    {
        PlayerPrefs.SetFloat("masterBrightness", _brightnessLvl);
        PlayerPrefs.SetInt("masterQuality", _qualityLvl);
        QualitySettings.SetQualityLevel(_qualityLvl);

        PlayerPrefs.SetInt("masterFullscreen", (_fullscreen ? 1 : 0));
        Screen.fullScreen = _fullscreen;

    }

    public void ResetBtn(string menuType)
    {
        if (menuType == "Audio")
        {
            AudioListener.volume = defaultVolume;
            volumeSlider.value = defaultVolume;
            volumeValueText.text = defaultVolume.ToString("0.0");
            volumeApply();
        }

        if (menuType == "Graphics")
        {
            brightnesSlider.value = defaultBrightness;
            brightnessValue.text = defaultBrightness.ToString("0.0");

            qualityDropdown.value = 1;
            QualitySettings.SetQualityLevel(1);

            fullscreenToggle.isOn = false;
            Screen.fullScreen = false;

            Resolution currentResolution = Screen.currentResolution;
            Screen.SetResolution(currentResolution.width, currentResolution.height, Screen.fullScreen);
            resolutionDropdown.value = resolutions.Length;
            graphicsApply();
    
        }
    }

}



