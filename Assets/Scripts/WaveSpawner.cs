using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
    public static WaveSpawner Instance;
    public EnemySpawner enemySpawner;
    public float timeBetweenWaves = 30f;
    public int baseEnemiesPerWave = 3;
    public int enemiesAddedPerWave = 2; 
    public int currentWave = 0;
    private float waveTimer;
    private bool waitingForWave = true;
    public float countdownDuration = 5f; 
    private float countdown;
    private bool countingDown = false;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        StartCountdown();
    }

    void Update()
    {
        if (countingDown)
        {
            countdown -= Time.deltaTime;
            if (countdown <= 0)
            {
                countingDown = false;
                SpawnWave();
            }
            return;
        }

        if (waitingForWave)
        {
            return;
        }

        waveTimer += Time.deltaTime;
        if (waveTimer >= timeBetweenWaves)
        {
            waveTimer = 0;
            StartCountdown();
        }
    }

    void StartCountdown()
    {
        countdown = countdownDuration;
        countingDown = true;
        waitingForWave = false;
    }

    void SpawnWave()
    {
        currentWave++;
        int enemyCount = baseEnemiesPerWave + (currentWave - 1) * enemiesAddedPerWave;
        enemySpawner.SpawnEnemies(enemyCount);
        Debug.Log($"Wave {currentWave} spawned with {enemyCount} enemies.");
    }
    public void SpawnBonus(int amount)
    {
        enemySpawner.SpawnEnemies(amount); //God Spawned extra enemies
    }

    void OnGUI() //UI
    {
        GUIStyle style = new GUIStyle();
        style.fontSize = 20;
        style.normal.textColor = Color.white;
        style.alignment = TextAnchor.UpperRight;
        float x = Screen.width - 210f;
        GUI.Label(new Rect(x, 10, 200, 30), $"Wave: {currentWave}", style);
        if (countingDown)
        {
            style.normal.textColor = Color.red;
            GUI.Label(new Rect(x, 40, 200, 30), $"Next wave: {countdown:F1}s", style);
        }
        else
        {
            style.normal.textColor = Color.yellow;
            float timeUntilNext = timeBetweenWaves - waveTimer;
            GUI.Label(new Rect(x, 40, 200, 30), $"Next wave: {timeUntilNext:F1}s", style);
        }
    }
}