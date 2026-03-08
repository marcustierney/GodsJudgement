using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip buttonClick;
    public AudioClip startGame;
    void PlaySound()
    {
        audioSource.PlayOneShot(buttonClick);
    }
    public void PlayGame()
    {
        audioSource.PlayOneShot(startGame);
        SceneManager.LoadScene("MainScene"); 
    }

    public void SelectDifficulty()
    {
        PlaySound();
        SceneManager.LoadScene("DifficultyScene");
    }

    public void OpenSettings()
    {
        PlaySound();
        SceneManager.LoadScene("SettingsScene");
    }

    public void OpenCredits()
    {
        PlaySound();
        SceneManager.LoadScene("CreditsScene");
    }

    public void BackToMenu()
    {
        PlaySound();
        SceneManager.LoadScene("MenuScene");
    }

    public void QuitGame()
    {
        PlaySound();
        Application.Quit();
    }
}