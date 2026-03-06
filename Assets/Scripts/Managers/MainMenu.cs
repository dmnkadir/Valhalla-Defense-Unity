using UnityEngine;
using UnityEngine.SceneManagement; // To change scenes 

public class MainMenu : MonoBehaviour
{
    // To be connected to the PLAY Button
    public void PlayGame()
    {
        SceneManager.LoadScene("SampleScene");
    }

    // To be connected to the EXIT Button
    public void QuitGame()
    {
        Debug.Log("Exiting game...");
        Application.Quit();
    }
}