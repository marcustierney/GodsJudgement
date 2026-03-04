using UnityEngine;

public class EnemyBrain : MonoBehaviour
{
    UnitMovement movement;
    UnitCombat combat;
    Health myHealth;
    public float priorityRange = 5f; 
    public float targetUpdateInterval = 0.5f;
    private float targetTimer;
    private GameObject currentTarget;

    void Start()
    {
        movement = GetComponent<UnitMovement>();
        combat = GetComponent<UnitCombat>();
        myHealth = GetComponent<Health>();
    }

    void Update()
    {
        if (myHealth != null && myHealth.isDead)
        {
            return;
        }
        targetTimer += Time.deltaTime;
        if (targetTimer >= targetUpdateInterval)
        {
            targetTimer = 0;
            currentTarget = FindBestTarget();
        }
        if (currentTarget == null)
        {
            currentTarget = FindBestTarget();
            return;
        }
        if (!IsTargetAlive(currentTarget))
        {
            currentTarget = null;
            return;
        }
        bool inRange = combat.TryAttack(currentTarget);
        if (!inRange)
        {
            movement.MoveTo(currentTarget.transform.position);
        }
        else
        {
            movement.StopMoving();
        }           
    }

    GameObject FindBestTarget()
    {
        //First priority nearest friendly unit within range
        GameObject nearestFriendly = FindNearestWithinRange("Friendly", priorityRange);
        if (nearestFriendly != null)
        {
            return nearestFriendly;
        }
        //Second priority nearest building within range
        GameObject nearestBuilding = FindNearestBuildingWithinRange(priorityRange);
        if (nearestBuilding != null)
        {
            return nearestBuilding;
        }
        //Third priority townhall (always target regardless of distance)
        return GameManager.Instance.townHall;
    }

    GameObject FindNearestWithinRange(string tag, float range)
    {
        GameObject[] candidates = GameObject.FindGameObjectsWithTag(tag);
        GameObject nearest = null;
        float closest = range; 
        foreach (var candidate in candidates)
        {
            Health h = candidate.GetComponent<Health>();
            if (h != null && h.isDead)
            {
                continue;
            }
            float dist = Vector3.Distance(transform.position, candidate.transform.position);
            if (dist < closest)
            {
                closest = dist;
                nearest = candidate;
            }
        }
        return nearest;
    }

    GameObject FindNearestBuildingWithinRange(float range)
    {
        GameObject[] buildings = GameObject.FindGameObjectsWithTag("Building");
        GameObject nearest = null;
        float closest = range; 
        foreach (var b in buildings)
        {
            BuildingHealth bh = b.GetComponent<BuildingHealth>();
            if (bh == null || bh.currentHealth <= 0)
            {
                continue;
            }
            float dist = Vector3.Distance(transform.position, b.transform.position);
            if (dist < closest)
            {
                closest = dist;
                nearest = b;
            }
        }
        return nearest;
    }

    bool IsTargetAlive(GameObject target)
    {
        if (target == null)
        {
            return false;
        }
        Health h = target.GetComponent<Health>();
        if (h != null)
        {
            return !h.isDead;
        }
        BuildingHealth bh = target.GetComponent<BuildingHealth>();
        if (bh != null)
        {
            return bh.currentHealth > 0;
        }
        BuildingHealth childBh = target.GetComponentInChildren<BuildingHealth>();
        if (childBh != null)
        {
            return childBh.currentHealth > 0;
        }
        return true;
    }
}