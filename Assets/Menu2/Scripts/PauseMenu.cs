using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [Header("UI Panels")]
    public GameObject pauseMenuPanel;
    public GameObject settingsContainer;
    public static bool isPaused = false;


    void Update()
    {
        // Check for the Escape key press
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                // If paused and not in settings, continue the game.
                // If in settings, the back button should handle it.
                if (pauseMenuPanel.activeInHierarchy)
                {
                    Continue();
                }
            }
            else
            {
                Pause();
            }
        }
    }

    public void Pause()
    {
        pauseMenuPanel.SetActive(true);
        Time.timeScale = 0f; // This freezes the game
        isPaused = true;

        // Unlock and show the cursor
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void Continue()
    {
        pauseMenuPanel.SetActive(false);
        settingsContainer.SetActive(false); // Ensure settings are closed too
        Time.timeScale = 1f; // This unfreezes the game
        isPaused = false;

        // Lock and hide the cursor for gameplay
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void OpenSettings()
    {
        pauseMenuPanel.SetActive(false);
        settingsContainer.SetActive(true);
    }

    public void CloseSettings()
    {
        settingsContainer.SetActive(false);
        pauseMenuPanel.SetActive(true);
    }

    public void QuitToMainMenu()
    {
        // IMPORTANT: Always reset time scale before leaving the scene
        Time.timeScale = 1f;
        isPaused = false;
        
        // Use the scene transition to go back
        SceneTransition.instance.LoadSceneWithTransition("MainMenu");
    }
}
