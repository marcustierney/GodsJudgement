using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance;
    public AudioClip menuMusic;
    public AudioClip mainMusic;
    public AudioSource musicSource;
    private string currentClipName = "";

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        PlayMusicForScene(SceneManager.GetActiveScene().name);
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        PlayMusicForScene(scene.name);
    }

    void PlayMusicForScene(string sceneName)
    {
        AudioClip clipToPlay = null;

        if (sceneName == "MenuScene" || sceneName == "DifficultyScene" || sceneName == "SettingsScene" || sceneName == "CreditsScene")
        {
            clipToPlay = menuMusic;
        }
        else
        {
            clipToPlay = mainMusic;
        }

        if (clipToPlay == null)
        {
            return;
        }

        if (musicSource.clip == clipToPlay && musicSource.isPlaying) //Dont restart if same clip is already playing
        {
            return;
        }
        musicSource.clip = clipToPlay;
        musicSource.loop = true;
        musicSource.Play();
        currentClipName = clipToPlay.name;
    }

    public void SetMusicVolume(float volume)
    {
        musicSource.volume = volume;
    }

    public float GetMusicVolume()
    {
        return musicSource.volume;
    }
}