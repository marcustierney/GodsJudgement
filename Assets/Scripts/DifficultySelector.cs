using UnityEngine;
using TMPro;

public class DifficultySelector : MonoBehaviour
{
    public TextMeshProUGUI currentDifficultyText;

    void Start()
    {
        UpdateDifficultyText(DifficultyData.Instance.selectedDifficulty);
    }

    public void SelectEasy() {
        SetDifficulty(Difficulty.Easy);
    }
    public void SelectMedium()
    {
        SetDifficulty(Difficulty.Medium);
    }
    public void SelectHard()
    {
        SetDifficulty(Difficulty.Hard);
    }

    void SetDifficulty(Difficulty difficulty)
    {
        DifficultyData.Instance.selectedDifficulty = difficulty;
        UpdateDifficultyText(difficulty);
        Debug.Log($"Difficulty set to {difficulty}");
    }

    void UpdateDifficultyText(Difficulty difficulty)
    {
        switch (difficulty)
        {
            case Difficulty.Easy:
                currentDifficultyText.text = "EASY";
                break;

            case Difficulty.Medium:
                currentDifficultyText.text = "MEDIUM";
                break;

            case Difficulty.Hard:
                currentDifficultyText.text = "HARD";
                break;
        }
    }
}