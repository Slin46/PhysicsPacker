using TMPro;
using UnityEngine;

public class Goal : MonoBehaviour
{
    public int totalBoxes = 5;
    private int completedBoxes = 0;

    public TextMeshProUGUI progressText;

    private RoundManager roundManager;

    void Start()
    {
        roundManager = FindFirstObjectByType<RoundManager>();
        UpdateUI();
    }
    private void OnTriggerEnter(Collider other)
{
    Debug.Log("GOAL HIT BY: " + other.name);

    BoxScript box = other.GetComponentInParent<BoxScript>();
    if (box == null)
    {
        Debug.Log("No BoxScript found in parent.");
        return;
    }

    if (!box.isPacked)
    {
        Debug.Log("Box not packed yet.");
        return;
    }

    completedBoxes++;
    UpdateUI();

    Debug.Log("DESTROYING BOX + ITEM: " + box.gameObject.name);

    // 🔹 Destroy packed item FIRST (child of boxInteriorPoint)
    if (box.boxInteriorPoint != null && box.boxInteriorPoint.childCount > 0)
    {
        Destroy(box.boxInteriorPoint.GetChild(0).gameObject);
    }

    // 🔹 Then destroy the box
    Destroy(box.gameObject);

    if (completedBoxes >= totalBoxes && roundManager != null)
    {
        roundManager.WinRound();
    }
}

    private void UpdateUI()
    {
        if (progressText != null)
            progressText.text = "Completed: " + completedBoxes + "/" + totalBoxes;
    }
}