using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    [SerializeField] private GameObject transitionCanvasPrefab;
    [SerializeField] private float fadeDuration = 1f;

    private CanvasGroup _fadeCanvasGroup;

    public bool IsTransitioning { get; private set; } = false;
    public static SceneChanger Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        DontDestroyOnLoad(gameObject);

        ServiceLocator.Instance.RegisterService(this);

        if (transitionCanvasPrefab != null)
        {
            var transitionCanvas = Instantiate(transitionCanvasPrefab);
            DontDestroyOnLoad(transitionCanvas);

            _fadeCanvasGroup = transitionCanvas.GetComponentInChildren<CanvasGroup>();

            if (_fadeCanvasGroup == null)
                Debug.LogError("Transition Canvas prefab needs a CanvasGroup component.");
        }
        else
        {
            Debug.LogWarning("SceneChanger has no transitionCanvasPrefab assigned!");
        }
    }

    public void ChangeScene(string sceneName)
    {
        if (!IsTransitioning)
            StartCoroutine(TransitionAndLoad(sceneName));

        FixPauseState();
    }

    public void ChangeScene(int sceneIndex)
    {
        if (!IsTransitioning)
            StartCoroutine(TransitionAndLoad(sceneIndex));

        FixPauseState();
    }

    private IEnumerator TransitionAndLoad(string sceneName)
    {
        IsTransitioning = true;

        if (_fadeCanvasGroup != null)
        {
            _fadeCanvasGroup.blocksRaycasts = true;
            yield return StartCoroutine(Fade(0f, 1f));
        }

        SceneManager.LoadScene(sceneName);
        yield return null;

        if (_fadeCanvasGroup != null)
        {
            yield return StartCoroutine(Fade(1f, 0f)); 
            _fadeCanvasGroup.blocksRaycasts = false;
        }

        IsTransitioning = false;
    }

    private IEnumerator TransitionAndLoad(int sceneIndex)
    {
        IsTransitioning = true;

        if (_fadeCanvasGroup != null)
        {
            _fadeCanvasGroup.blocksRaycasts = true;
            yield return StartCoroutine(Fade(0f, 1f));
        }

        SceneManager.LoadScene(sceneIndex);
        yield return null;

        if (_fadeCanvasGroup != null)
        {
            yield return StartCoroutine(Fade(1f, 0f));
            _fadeCanvasGroup.blocksRaycasts = false;
        }

        IsTransitioning = false;
    }

    private void FixPauseState()
    {
        if (PauseManager.IsPaused)
        {
            PauseManager.SetPaused(false);
        }
        if (PauseManager.IsPausedCinematic)
        {
            PauseManager.SetPausedCinematic(false);
        }
    }

    private IEnumerator Fade(float start, float end)
    {
        float elapsed = 0f;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.unscaledDeltaTime;
            float t = Mathf.Clamp01(elapsed / fadeDuration);
            _fadeCanvasGroup.alpha = Mathf.Lerp(start, end, t);
            yield return null;
        }

        _fadeCanvasGroup.alpha = end;
    }
}
