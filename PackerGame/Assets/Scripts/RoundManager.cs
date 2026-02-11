using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RoundManager : MonoBehaviour
{
    public static RoundManager Instance;

    [Header("Round Settings")]
    public float roundTime = 120f;       // 2 minutes
    private float timer;
    private bool roundActive = true;

    [Header("Boxes")]
    public int boxesCompleted = 0;
    public int boxesToWin = 5;

    [Header("UI")]
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI timesUpText;

    [Header("End Scene")]
    public string resultSceneName = "ResultScene";
    private string winOrLose;

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
            EndRound("Time's up!");
            winOrLose = "LOSE";
        }

        UpdateTimerUI();
    }

    private void UpdateTimerUI()
    {
        if (timerText != null)
        {
            int minutes = Mathf.FloorToInt(timer / 60f);
            int seconds = Mathf.FloorToInt(timer % 60f);
            timerText.text = $"Time Left: {minutes:00}:{seconds:00}";
        }
    }

    /// <summary>
    /// Called by BoxScript when a box is successfully packed.
    /// </summary>
    /// <param name="box">The completed box</param>
    public void OnBoxCompleted(BoxScript box)
    {
        boxesCompleted++;
        Debug.Log($"Box completed! Total: {boxesCompleted}");

        // Optional: spawn a new replacement box
        if (boxesCompleted < boxesToWin)
        {
            ItemGenerator generator = FindFirstObjectByType<ItemGenerator>();
            if (generator != null)
                generator.SpawnReplacementBox();
        }

        if (boxesCompleted >= boxesToWin)
        {
            WinRound();
        }
    }

    /// <summary>
    /// Ends the round as a win
    /// </summary>
    public void WinRound()
    {
        if (!roundActive) return;

        winOrLose = "WIN";
        EndRound("Good Job!");
    }

    /// <summary>
    /// Ends the round and shows times up or win message
    /// </summary>
    /// <param name="reason">Message to display</param>
    private void EndRound(string reason)
    {
        if (!roundActive) return;

        roundActive = false;

        if (timesUpText != null)
        {
            timesUpText.text = reason;
            timesUpText.gameObject.SetActive(true);
        }

        Time.timeScale = 0f; // freeze game

        StartCoroutine(LoadResultScene());
    }

    private IEnumerator LoadResultScene()
    {
        yield return new WaitForSecondsRealtime(1f);

        Time.timeScale = 1f; // unfreeze for scene load
        PlayerPrefs.SetString("RESULT", winOrLose);
        SceneManager.LoadScene(resultSceneName);
    }
}