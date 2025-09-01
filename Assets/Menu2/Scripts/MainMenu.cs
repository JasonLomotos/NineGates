using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [Header("Menu Containers")]
    public GameObject mainMenuContainer;
    public GameObject settingsContainer;

    void Start()
    {
        // Important: Ensure time is normal when the main menu starts.
        Time.timeScale = 1f;

        mainMenuContainer.SetActive(true);
        settingsContainer.SetActive(false);
    }

    public void NewGame()
    {
        // We call the SceneTransition singleton to handle the loading process.
        // Replace "YourGameSceneName" with your actual scene name.
        SceneTransition.instance.LoadSceneWithTransition("Main scene");
    }

    public void LoadGame()
    {
        Debug.Log("Main scene");
    }

    public void OpenSettings()
    {
        if (mainMenuContainer != null && settingsContainer != null)
        {
            mainMenuContainer.SetActive(false);
            settingsContainer.SetActive(true);
        }
    }



    public void CloseSettings()
    {
        if (mainMenuContainer != null && settingsContainer != null)
        {
            mainMenuContainer.SetActive(true);
            settingsContainer.SetActive(false);
        }
    }

    public void ExitGame()
    {
        Debug.Log("Exit Game button clicked!");
        Application.Quit();
    }
}

