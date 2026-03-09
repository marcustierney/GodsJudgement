using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using TMPro;
using System.Collections;

public class PauseMenu : MonoBehaviour
{
    public static PauseMenu Instance;
    public GameObject pausePanel;   
    public Slider masterVolumeSlider;
    public Slider musicVolumeSlider;
    public TextMeshProUGUI masterVolumeLabel;
    public TextMeshProUGUI musicVolumeLabel;
    public AudioClip buttonClickSound;
    private AudioSource audioSource;
    private bool isPaused = false;

    void Awake()
    {
        Instance = this;
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    void Start()
    {
        pausePanel.SetActive(false);
        if (AudioManager.Instance != null)
        {
            masterVolumeSlider.value = AudioManager.Instance.GetMasterVolume();
            musicVolumeSlider.value = AudioManager.Instance.GetMusicVolume();
        }
        masterVolumeSlider.onValueChanged.AddListener(OnMasterVolumeChanged);
        musicVolumeSlider.onValueChanged.AddListener(OnMusicVolumeChanged);
        UpdateLabels();
    }

    void Update()
    {
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            if (isPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Pause()
    {
        isPaused = true;
        pausePanel.SetActive(true);
        Time.timeScale = 0f; //Freeze game
        PlayClick();
    }

    public void Resume()
    {
        isPaused = false;
        pausePanel.SetActive(false);
        Time.timeScale = 1f; //Unfreeze game
        PlayClick();
    }

    public void OnClickResume()
    {
        Resume();
    }

    public void OnClickMainMenu()
    {
        Time.timeScale = 1f; //Reset timescale before quiting
        StartCoroutine(LoadMenuAfterSound());
    }

    IEnumerator LoadMenuAfterSound()
    {
        PlayClick();
        yield return new WaitForSecondsRealtime(buttonClickSound.length);
        SceneManager.LoadScene("MenuScene");
    }

    void OnMasterVolumeChanged(float value)
    {
        AudioManager.Instance?.SetMasterVolume(value);
        UpdateLabels();
    }

    void OnMusicVolumeChanged(float value)
    {
        AudioManager.Instance?.SetMusicVolume(value);
        UpdateLabels();
    }

    void UpdateLabels()
    {
        masterVolumeLabel.text = $"Master: {Mathf.RoundToInt(masterVolumeSlider.value * 100)}%";
        musicVolumeLabel.text = $"Music: {Mathf.RoundToInt(musicVolumeSlider.value * 100)}%";
    }

    void PlayClick()
    {
        audioSource.PlayOneShot(buttonClickSound);
    }
}