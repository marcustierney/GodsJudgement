using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public GameObject townHall;
    public bool gameOver = false;
    public float gameTime = 0f;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Update()
    {
        if (gameOver)
        {
            return;
        }
        gameTime += Time.deltaTime;
        CheckLoseCondition();
    }

    void CheckLoseCondition()
    {
        if (townHall == null)
        {
            LoseGame();
        }
    }

    public void LoseGame()
    {
        gameOver = true;
        SceneManager.LoadScene("LoseScene");
    }
}