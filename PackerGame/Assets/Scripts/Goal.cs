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

    Rigidbody rb = other.attachedRigidbody;
    if (rb == null) return;

    BoxScript box = rb.GetComponent<BoxScript>();
    if (box == null) return;
    if (!box.isPacked) return;

    completedBoxes++;
    UpdateUI();

    Debug.Log("DESTROYING: " + rb.gameObject.name);

    Destroy(rb.gameObject);

    if (completedBoxes >= totalBoxes && roundManager != null)
        roundManager.WinRound();
}


    private void UpdateUI()
    {
        if (progressText != null)
            progressText.text = "Completed: " + completedBoxes + "/" + totalBoxes;
    }
}