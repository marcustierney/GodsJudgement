using UnityEngine;

public class ResourceDisplay : MonoBehaviour
{
    void OnGUI()
    {
        GUILayout.Label("Food: " + ResourceManager.Instance.food);
        GUILayout.Label("Wood: " + ResourceManager.Instance.wood);
        GUILayout.Label("Troops: " + ResourceManager.Instance.troops);
    }
}