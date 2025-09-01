using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{
    public static SceneTransition instance;

    [Header("Transition Settings")]
    public CanvasGroup transitionCanvasGroup;
    public float fadeDuration = 1.5f;
    public float minTimeScale = 0.1f;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        if (transitionCanvasGroup != null)
        {
            transitionCanvasGroup.alpha = 0f;
            transitionCanvasGroup.blocksRaycasts = false;
        }
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public void LoadSceneWithTransition(string sceneName)
    {
        // --- DEBUG CHECK ---
        Debug.Log("LoadSceneWithTransition called for scene: " + sceneName);
        if (transitionCanvasGroup == null)
        {
            Debug.LogError("FATAL: Transition Canvas Group is not assigned in the Inspector on the SceneTransition script!");
            return; // Stop if the reference is missing
        }
        StartCoroutine(TransitionRoutine(sceneName));
    }

    private IEnumerator TransitionRoutine(string sceneName)
    {
        // --- DEBUG LOG ---
        Debug.Log("TransitionRoutine started. Fading out...");
        transitionCanvasGroup.blocksRaycasts = true;

        float timer = 0f;
        while (timer < fadeDuration)
        {
            transitionCanvasGroup.alpha = Mathf.Lerp(0f, 1f, timer / fadeDuration);
            Time.timeScale = Mathf.Lerp(1f, minTimeScale, timer / fadeDuration);
            timer += Time.unscaledDeltaTime;
            yield return null;
        }
        transitionCanvasGroup.alpha = 1f;
        Time.timeScale = minTimeScale;

        // --- DEBUG LOG ---
        Debug.Log("Fade out complete. Loading scene asynchronously.");
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        asyncLoad.allowSceneActivation = false;

        while (!asyncLoad.isDone)
        {
            if (asyncLoad.progress >= 0.9f)
            {
                break;
            }
            yield return null;
        }

        asyncLoad.allowSceneActivation = true;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // --- DEBUG LOG ---
        Debug.Log("Scene loaded: " + scene.name);
        if (scene.name != "MainMenu")
        {
            StartCoroutine(FadeInRoutine());
        }
        else
        {
            transitionCanvasGroup.alpha = 0f;
            transitionCanvasGroup.blocksRaycasts = false;
        }
    }

    private IEnumerator FadeInRoutine()
    {
        // --- DEBUG LOG ---
        Debug.Log("FadeInRoutine started. Fading in...");
        float timer = 0f;
        while (timer < fadeDuration)
        {
            transitionCanvasGroup.alpha = Mathf.Lerp(1f, 0f, timer / fadeDuration);
            Time.timeScale = Mathf.Lerp(minTimeScale, 1f, timer / fadeDuration);
            timer += Time.unscaledDeltaTime;
            yield return null;
        }
        transitionCanvasGroup.alpha = 0f;
        Time.timeScale = 1f;

        transitionCanvasGroup.blocksRaycasts = false;
        // --- DEBUG LOG ---
        Debug.Log("Fade in complete.");
    }
}

