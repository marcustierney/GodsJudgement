using UnityEngine;

public class TroopStats : MonoBehaviour
{
    public float damageReduction = 0f; //0 = no reduction 1 = immune
    private float maxDamageReduction = 0.75f;

    public void ApplyDamageReduction(float amount)
    {
        damageReduction = Mathf.Min(damageReduction + amount, maxDamageReduction);
    }

    public float ModifyIncomingDamage(float incomingDamage)
    {
        return incomingDamage * (1f - damageReduction);
    }
}