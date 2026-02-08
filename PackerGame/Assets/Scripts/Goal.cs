using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class Goal : MonoBehaviour
{
    //amount of boxes and how much r completed
    public int totalBoxes = 5;
    private int completedBoxes = 0;

    //text UI to show how much boxes r completed
    public TextMeshProUGUI progressText;

    private RoundManager roundManager;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        roundManager = FindFirstObjectByType<RoundManager>();
        UpdateUI();
    }

    private void OnTriggerEnter(Collider other)
    {
        //grab the boxscript
        BoxScript box = other.GetComponent<BoxScript>();
        if (box == null) return;

        // Only accept packed boxes
        if (!box.isPacked) return;
        //avoid double counting
        box.enabled = false;

        completedBoxes++;
        UpdateUI();

        Destroy(other.gameObject);

        if (completedBoxes >= totalBoxes)
        {
            //call round manage
            if (roundManager != null)
            {
                RoundManager.Instance.WinRound();
            }
            Debug.Log("All boxes completed!");
        }
    }
    private void UpdateUI()
    {
        if (progressText != null)
            progressText.text = "Completed: "+ completedBoxes + "/" + totalBoxes;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
