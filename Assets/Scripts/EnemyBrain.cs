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
    private bool currentlyAttacking = false;

    void Start()
    {
        movement = GetComponent<UnitMovement>();
        combat = GetComponent<UnitCombat>();
        myHealth = GetComponent<Health>();
        transform.rotation = Quaternion.Euler(0, -90, 0);
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
            currentlyAttacking = false;
            return;
        }
        if (!IsTargetAlive(currentTarget))
        {
            currentTarget = null;
            currentlyAttacking = false;
            return;
        }
        bool inRange = combat.TryAttack(currentTarget);
        currentlyAttacking = inRange;
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
            Wall wall = b.GetComponent<Wall>();
            if (bh == null && wall == null)
            {
                continue;
            }
            if (bh != null && bh.currentHealth <= 0)
            {
                continue;
            }
            if (wall != null && wall.currentHealth <= 0)
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
        Wall wall = target.GetComponent<Wall>();
        if (wall != null)
        {
            return wall.currentHealth > 0;
        }
        return true;
    }

    public bool IsAttacking()
    {
        Debug.Log($"{gameObject.name} IsAttacking: {currentlyAttacking}");
        return currentlyAttacking;
    }
}