using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Menu BGM")]
    public AudioSource menuBgm;

    [Header("Gameplay scene name")]
    public string gameplaySceneName = "Scene1";

    void Awake()
    {
        // Singleton
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // 🔹 stay across scenes
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    void Start()
    {
        PlayMenuBGM();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // 🎵 Play menu music
    public void PlayMenuBGM()
    {
        if (menuBgm != null && !menuBgm.isPlaying)
            menuBgm.Play();
    }

    // ⛔ Stop when gameplay scene loads
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == gameplaySceneName)
        {
            if (menuBgm != null)
                menuBgm.Stop();
        }
        else
        {
            PlayMenuBGM();
        }
    }
}