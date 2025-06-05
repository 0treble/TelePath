using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip clickSound;

    public void PlayGame()
    {
        PlayClickSound();
        SceneManager.LoadScene("Game");
    }

    public void PlayGameCompact()
    {
        PlayClickSound();
        SceneManager.LoadScene("Game (compact)");
    }

    public void Calibration()
    {
        PlayClickSound();
        SceneManager.LoadScene("Calibration");
    }

    public void Back2MainMenu()
    {
        PlayClickSound();
        SceneManager.LoadScene("Main Menu");
    }

    public void QuitGame()
    {
        PlayClickSound();
        Debug.Log("Quit!");
        Application.Quit();
    }

    private void CheckForMenu()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            PlayClickSound();
            SceneManager.LoadScene("Calibration");
        }
        else if (Input.GetKeyDown(KeyCode.P))
        {
            PlayClickSound();
            SceneManager.LoadScene("Game");
        }
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            PlayClickSound();
            SceneManager.LoadScene("Main Menu");
        }
    }

    private void Update()
    {
        CheckForMenu();
    }

    private void PlayClickSound()
    {
        if (audioSource != null && clickSound != null)
        {
            audioSource.PlayOneShot(clickSound);
        }
    }
}
