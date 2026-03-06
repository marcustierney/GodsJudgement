using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameOver : MonoBehaviour
{
    public TextMeshProUGUI moralityLabel;
    public TextMeshProUGUI styleLabel;
    public TextMeshProUGUI consumptionLabel;
    public TextMeshProUGUI moralityPercent;
    public TextMeshProUGUI stylePercent;
    public TextMeshProUGUI consumptionPercent;
    public TextMeshProUGUI wavesReachedText;
    public UnityEngine.UI.Image moralityBar;
    public UnityEngine.UI.Image styleBar;
    public UnityEngine.UI.Image consumptionBar;

    void Start()
    {
        GameOverData d = GameOverData.Instance;
        moralityLabel.text = $"Morality ({d.morality}):";
        styleLabel.text = $"Style ({d.style}):";
        consumptionLabel.text = $"Consumption ({d.consumption}):";
        moralityPercent.text = $"{d.moralitySatisfaction * 100f:F0}%";
        stylePercent.text = $"{d.styleSatisfaction * 100f:F0}%";
        consumptionPercent.text = $"{d.consumptionSatisfaction * 100f:F0}%";
        moralityBar.fillAmount = d.moralitySatisfaction;
        styleBar.fillAmount = d.styleSatisfaction;
        consumptionBar.fillAmount = d.consumptionSatisfaction;

        wavesReachedText.text = $"Waves Survived: {d.wavesReached}";
    }
    public void PlayGame()
    {
        SceneManager.LoadScene("MainScene");
    }

    public void EnterMainMenu()
    {
        SceneManager.LoadScene("MenuScene");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}