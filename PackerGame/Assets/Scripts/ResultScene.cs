using TMPro;
using UnityEngine;

public class ResultScene : MonoBehaviour
{
    public TextMeshProUGUI resultText;
    public GameObject winImage;
    public GameObject loseImage;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        string result = PlayerPrefs.GetString("RESULT", "LOSE");
        //color coding the result strings in hexcode
        ColorUtility.TryParseHtmlString("#578F2D", out Color winGreen); //green
        ColorUtility.TryParseHtmlString("#AA3232", out Color loseRed);  //red

        //if result is win then it will say pay day in green and the win image will activate
        if (result == "WIN")
        {
            resultText.text = "PAY DAY!";
            resultText.color = winGreen;
            winImage.SetActive(true);
            loseImage.SetActive(false);
        }
        //else losing it will say your fired and lose image will activate instead
        else
        {
            resultText.text = "YOU'RE FIRED!";
            resultText.color = loseRed;
            winImage.SetActive(false);
            loseImage.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
