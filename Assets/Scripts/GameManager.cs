using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Core References")]
    public GameObject townHall;

    [Header("Game State")]
    public bool gameOver = false;
    public float gameTime = 0f;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    void Update()
    {
        if (gameOver)
            return;

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
        Debug.Log("Game Over Town Hall Destroyed");
    }
}