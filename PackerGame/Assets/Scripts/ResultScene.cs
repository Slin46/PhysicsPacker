using TMPro;
using UnityEngine;

public class ResultScene : MonoBehaviour
{
    public TextMeshProUGUI resultText;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        string result = PlayerPrefs.GetString("RESULT", "LOSE");

        if (result == "WIN")
            resultText.text = "YOU WIN!";
        else
            resultText.text = "YOU LOSE!";
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
