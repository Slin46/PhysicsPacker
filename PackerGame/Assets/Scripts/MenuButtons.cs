using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuButtons : MonoBehaviour
{
    void Start()
    {
    }
    public static class SceneHistory
    {
        //save the current scene name its on
        public static string previousScene = "";
    }
    
    public void PLAY()
    {
        // Load the first scene
        SceneManager.LoadScene("Instructions");
    }
    public void NEXT()
    {
        // Load the first scene
        SceneManager.LoadScene("Scene1");
    }
    public void EXIT()
    {
        //pressing exit will quit the application as a whole
        Application.Quit();
        Debug.Log("Game is quitting...");
    }
    public void Home()
    {
        //destroy everything
        SceneHistory.previousScene = "";

        //restarts game
        SceneManager.LoadScene(0);
        Debug.Log("Restarting...");

    }
    
}
