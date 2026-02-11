using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RoundManager : MonoBehaviour
{
    public static RoundManager Instance;

    [Header("Game Settings")]
    public int boxesToWin = 5;              // Number of boxes to pack to win
    public float roundTime = 120f;          // Round duration in seconds

    private float timer;
    private bool roundActive = true;
    private int boxesCompleted = 0;

    [Header("UI")]
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI timesUpText;

    [Header("Result")]
    public string winOrLose;
    public string resultSceneName = "ResultScene";

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        timer = roundTime;
        UpdateTimerUI();

        if (timesUpText != null)
            timesUpText.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (!roundActive) return;

        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            timer = 0f;
            EndRound("Time's up!", "LOSE");
        }

        UpdateTimerUI();
    }

    private void UpdateTimerUI()
    {
        if (timerText != null)
        {
            int minutes = Mathf.FloorToInt(timer / 60f);
            int seconds = Mathf.FloorToInt(timer % 60f);
            timerText.text = string.Format("Time Left: {0:00}:{1:00}", minutes, seconds);
        }
    }

    /// <summary>
    /// Called by BoxScript when a box is successfully packed
    /// </summary>
    public void OnBoxCompleted(BoxScript box)
    {
        if (!roundActive) return;

        boxesCompleted++;
        Debug.Log($"Box completed! Total: {boxesCompleted}/{boxesToWin}");

        // Spawn replacement box if needed
        ItemGenerator generator = FindFirstObjectByType<ItemGenerator>();
        if (generator != null)
        {
            generator.SpawnReplacementBox();
        }

        if (boxesCompleted >= boxesToWin)
        {
            WinRound();
        }
    }

    /// <summary>
    /// Called when player completes all boxes
    /// </summary>
    public void WinRound()
    {
        EndRound("You win!", "WIN");
    }

    /// <summary>
    /// Ends the round and shows result
    /// </summary>
    private void EndRound(string message, string result)
    {
        roundActive = false;
        winOrLose = result;

        if (timesUpText != null)
        {
            timesUpText.text = message;
            timesUpText.gameObject.SetActive(true);
        }

        // Freeze game
        Time.timeScale = 0f;

        StartCoroutine(LoadResultScene());
    }

    private IEnumerator LoadResultScene()
    {
        yield return new WaitForSecondsRealtime(1f);

        Time.timeScale = 1f;
        PlayerPrefs.SetString("RESULT", winOrLose);

        SceneManager.LoadScene(resultSceneName);
    }
}