using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Menu BGM")]
    public AudioSource menuBgm;

    [Header("Gameplay scene name")]
    public string gameplaySceneName = "Scene1";

    private bool hasPlayedMenuMusic = false; // ← NEW

    void Awake()
    {
        // Singleton
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    void Start()
    {
        PlayMenuBGMOnce();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // 🎵 Play only ONE time ever
    void PlayMenuBGMOnce()
    {
        if (menuBgm != null && !hasPlayedMenuMusic)
        {
            menuBgm.loop = false; // extra safety
            menuBgm.Play();
            hasPlayedMenuMusic = true;
        }
    }

    // ⛔ Stop when gameplay scene loads
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == gameplaySceneName)
        {
            if (menuBgm != null && menuBgm.isPlaying)
                menuBgm.Stop();
        }
        // ❌ DO NOT replay music in other scenes
    }
}