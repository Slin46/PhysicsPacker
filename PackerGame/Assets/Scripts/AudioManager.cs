using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Menu BGM")]
    public AudioSource menuBgm;

    [Header("Gameplay scene name")]
    //gameplay scene
    public string gameplaySceneName = "Scene1"; 

    void Awake()
    {
        // Singleton pattern
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // persist across scenes
        }
        else
        {
            Destroy(gameObject); // destroy duplicates
            return;
        }
    }

    void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;

        // If the first scene is a menu scene, start music
        PlayMenuMusicIfMenuScene(SceneManager.GetActiveScene().name);
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Stop menu music in gameplay
        if (scene.name == gameplaySceneName)
        {
            StopMenuMusic();
        }
        else
        {
            // Start menu music in start/result/home scenes
            PlayMenuMusicIfMenuScene(scene.name);
        }
    }

    private void PlayMenuMusicIfMenuScene(string sceneName)
    {
        // Only play if not already playing
        if (menuBgm != null && !menuBgm.isPlaying && sceneName != gameplaySceneName)
        {
            menuBgm.loop = true;
            menuBgm.Play();
        }
    }

    private void StopMenuMusic()
    {
        if (menuBgm != null && menuBgm.isPlaying)
            menuBgm.Stop();
    }
}