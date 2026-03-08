using UnityEngine;
using TMPro;

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
    public TextMeshProUGUI waveText;
    public TextMeshProUGUI nextWaveText;

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
            UpdateUI();
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
        UpdateUI();
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
        UpdateUI();
    }

    void SpawnWave()
    {
        currentWave++;
        int enemyCount = baseEnemiesPerWave + (currentWave - 1) * enemiesAddedPerWave;
        enemySpawner.SpawnEnemies(enemyCount);
        Debug.Log($"Wave {currentWave} spawned with {enemyCount} enemies.");
        UpdateUI();
    }
    public void SpawnBonus(int amount)
    {
        enemySpawner.SpawnEnemies(amount); //God Spawned extra enemies
    }

    void UpdateUI()
    {
        if (waveText != null)
        {
            waveText.text = $"Wave: {currentWave}";
        }

        if (nextWaveText != null)
        {
            if (countingDown)
            {
                nextWaveText.text = $"Next wave: {countdown:F1}s";
            }
            else
            {
                float timeUntilNext = timeBetweenWaves - waveTimer;
                nextWaveText.text = $"Next wave: {timeUntilNext:F1}s";
            }
        }
    }
}