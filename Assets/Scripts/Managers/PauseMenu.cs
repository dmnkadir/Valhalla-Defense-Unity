using UnityEngine;
using UnityEngine.SceneManagement; // To change scenes (Retry/Menu)

public class PauseMenu : MonoBehaviour
{
    public GameObject ui; // Pause Panel (Black screen)

    void Start()
    {
        // Force close the menu when the game starts
        if (ui != null)
        {
            ui.SetActive(false);
        }

        // Make sure time is running
        Time.timeScale = 1f;
    }

    void Update()
    {
        // If the ESC key is pressed
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Toggle();
        }
    }

    public void Toggle()
    {
        ui.SetActive(!ui.activeSelf); // Toggle the PANEL state

        if (ui.activeSelf) // IF PANEL IS OPEN: Stop Time (0)
        {
            Time.timeScale = 0f;
        }
        else // IF PANEL IS CLOSED: Resume Time (1)
        {
            Time.timeScale = 1f;
        }
    }


    public void Resume()
    {
        Toggle(); // Close the menu and continue
    }

    public void Retry()
    {
        // Fix time and reload the scene from the beginning
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void Menu()
    {
        Time.timeScale = 1f;
        Debug.Log("Returning to Main Menu...");

        SceneManager.LoadScene("MainMenu");
    }
}