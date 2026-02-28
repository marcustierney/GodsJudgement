using UnityEngine;

public class Farm : MonoBehaviour
{
    public float foodPerSecond = 2f;

    void Update()
    {
        ResourceManager.Instance.AddFood(foodPerSecond * Time.deltaTime);
    }
}