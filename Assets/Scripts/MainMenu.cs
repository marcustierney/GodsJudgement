using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class MainMenu : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip buttonClick;
    public AudioClip startGame;
    void PlaySoundAndLoad(AudioClip clip, string scene)
    {
        StartCoroutine(PlayThenLoad(clip, scene));
    }

    IEnumerator PlayThenLoad(AudioClip clip, string scene)
    {
        audioSource.PlayOneShot(clip);
        yield return new WaitForSeconds(clip.length);
        SceneManager.LoadScene(scene);
    }

    public void PlayGame()
    {
        PlaySoundAndLoad(startGame, "MainScene");
    }

    public void SelectDifficulty()
    {
        PlaySoundAndLoad(buttonClick, "DifficultyScene");
    }

    public void OpenSettings()
    {
        PlaySoundAndLoad(buttonClick, "SettingsScene");
    }

    public void OpenCredits()
    {
        PlaySoundAndLoad(buttonClick, "CreditsScene");
    }

    public void BackToMenu()
    {
        PlaySoundAndLoad(buttonClick, "MenuScene");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}