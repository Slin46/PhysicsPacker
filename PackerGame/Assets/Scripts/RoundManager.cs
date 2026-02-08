using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RoundManager : MonoBehaviour
{
    public static RoundManager Instance;
    public ItemType currentOrder;
    public ItemType[] possibleOrders;
    //game is 2 min long
    public float roundTime = 120f;
    private float timer;
    private bool roundActive = true;

    public TextMeshProUGUI timerText;
    public TextMeshProUGUI timesUpText;

    //end scene with win and lose string
    public string winOrLose;
    public string resultSceneName = "ResultScene";
    public string currentItemOrder;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        timer = roundTime;
        UpdateTimerUI();
        GenerateNewOrder();

        if (timesUpText != null)
            timesUpText.gameObject.SetActive(false);
 
    }
    void Awake()
    {
    Instance = this;
    }
    // Update is called once per frame
    void Update()
    {
        if (roundActive)
        {
            //if timer is less than or equal to 0 it displays 0
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                timer = 0;
                EndRound("Time's up!");
            }
            UpdateTimerUI();
        }
    }
    public void EndRound(string reason)
    {
        if (!roundActive) return;
        roundActive = false;

        // Show TIME'S UP
        if (timesUpText != null)
        {
            timesUpText.text = reason;
            timesUpText.gameObject.SetActive(true);
        }

        // Freeze everything
        Time.timeScale = 0f;

        // Start delayed scene load
        StartCoroutine(LoadResultScene());
    }
    IEnumerator LoadResultScene()
    {
        // Wait 3 real seconds (ignores timeScale)
        yield return new WaitForSecondsRealtime(1f);

        // Unfreeze
        Time.timeScale = 1f;

        // Pass win/lose result
        PlayerPrefs.SetString("RESULT", winOrLose);

        SceneManager.LoadScene(resultSceneName);
    }

    private void UpdateTimerUI()
    {
        if (timerText != null)
        {
            //timer counting down
            int minutes = Mathf.FloorToInt(timer / 60f);
            int seconds = Mathf.FloorToInt(timer % 60f);
            timerText.text = string.Format("Time Left: {0:00}:{1:00}", minutes, seconds);
        }
    }
    public void GenerateNewOrder()
    {
    int index = Random.Range(0, possibleOrders.Length);
    currentOrder = possibleOrders[index];

    Debug.Log("NEW ORDER: " + currentOrder);
    }
    public void OnItemPacked(string itemName)
    {
    Debug.Log("Packed item: " + itemName);

    // Compare packed item with required order
    if (itemName == currentOrder)
    {
        Debug.Log("Correct item packed!");
        GenerateNewOrder();   // go to next order
    }
    else
    {
        Debug.Log("Wrong item packed!");
        // Optional: penalty, sound, etc.
    }
    }
}