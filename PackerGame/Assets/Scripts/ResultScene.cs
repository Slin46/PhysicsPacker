using TMPro;
using UnityEngine;

public class ResultScene : MonoBehaviour
{
    public TextMeshProUGUI resultText;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        string result = PlayerPrefs.GetString("RESULT", "LOSE");
        //color coding the result strings in hexcode
        ColorUtility.TryParseHtmlString("#578F2D", out Color winGreen); //green
        ColorUtility.TryParseHtmlString("#AA3232", out Color loseRed);  //red

        if (result == "WIN")
        {
            resultText.text = "PAY DAY!";
            resultText.color = winGreen;
        }
        else
        {
            resultText.text = "YOU'RE FIRED!";
            resultText.color = loseRed;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
