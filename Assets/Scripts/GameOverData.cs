using UnityEngine;

public class GameOverData : MonoBehaviour
{
    public static GameOverData Instance;

    public float moralitySatisfaction;
    public float styleSatisfaction;
    public float consumptionSatisfaction;
    public MoralityType morality;
    public StyleType style;
    public ConsumptionType consumption;
    public int wavesReached;

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