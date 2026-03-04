using UnityEngine;
public class Lumbermill : MonoBehaviour
{
    public float woodPerSecond = 5f;
    private float timer;

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= 1f)
        {
            timer = 0;
            ResourceManager.Instance.AddWood(woodPerSecond);
        }
    }
}