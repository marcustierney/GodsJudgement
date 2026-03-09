using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    public float masterVolume = 1f;
    public float musicVolume = 1f;

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        masterVolume = PlayerPrefs.GetFloat("MasterVolume", 1f);
        musicVolume = PlayerPrefs.GetFloat("MusicVolume", 1f);
        ApplyVolumes();
    }

    public void SetMasterVolume(float volume)
    {
        masterVolume = volume;
        ApplyVolumes();
        PlayerPrefs.SetFloat("MasterVolume", volume);
    }

    public void SetMusicVolume(float volume)
    {
        musicVolume = volume;
        ApplyVolumes();
        PlayerPrefs.SetFloat("MusicVolume", volume);
    }

    void ApplyVolumes()
    {
        AudioListener.volume = masterVolume;
        MusicManager.Instance.SetMusicVolume(musicVolume);
    }

    public float GetMasterVolume()
    {
        return masterVolume;
    }
    public float GetMusicVolume()
    {
        return musicVolume;
    }
}