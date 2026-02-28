using System.Collections.Generic;
using UnityEngine;

public class UtilityBrain : MonoBehaviour
{
    public List<UtilityAction> actions = new List<UtilityAction>();
    public float thinkInterval = 0.25f;
    private float timer;
    private AIContext context;
    private UtilityAction currentAction;
    private Health health;

    void Start()
    {
        health = GetComponent<Health>();
        context = new AIContext();
        context.self = gameObject;
        context.townHall = GameManager.Instance.townHall;
        actions.Add(new AttackEnemyAction());
        actions.Add(new DefendTownHallAction());
        actions.Add(new IdleAction());
    }

    void Update()
    {
        UpdateContext();

        timer += Time.deltaTime;
        if (timer >= thinkInterval)
        {
            timer = 0;
            ChooseBestAction();
        }

        if (currentAction != null)
        {
            currentAction.Execute(context);
        }
    }

    void ChooseBestAction()
    {
        float bestScore = 0;
        UtilityAction best = null;

        foreach (var action in actions)
        {
            float score = action.CalculateUtility(context);
            if (score > bestScore)
            {
                bestScore = score;
                best = action;
            }
        }

        currentAction = best;
    }

    void UpdateContext()
    {
        context.health = health.currentHealth;
        context.maxHealth = health.maxHealth;
        context.nearestEnemy = FindNearestEnemy();
        if (context.nearestEnemy != null)
        {
            context.nearestEnemyDistance = Vector3.Distance(transform.position, context.nearestEnemy.transform.position);
        }
        else
        {
            context.nearestEnemyDistance = Mathf.Infinity;
        }
        context.distanceToTownHall = Vector3.Distance(transform.position, context.townHall.transform.position);
    }

    GameObject FindNearestEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject nearest = null;
        float closest = Mathf.Infinity;
        foreach (var enemy in enemies)
        {
            float dist = Vector3.Distance(transform.position, enemy.transform.position);
            if (dist < closest)
            {
                closest = dist;
                nearest = enemy;
            }
        }
        return nearest;
    }
}