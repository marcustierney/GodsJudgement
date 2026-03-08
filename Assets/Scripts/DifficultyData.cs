using UnityEngine;

public enum Difficulty { Easy, Medium, Hard }

public class DifficultyData : MonoBehaviour
{
    public static DifficultyData Instance;
    public Difficulty selectedDifficulty = Difficulty.Easy;

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
}