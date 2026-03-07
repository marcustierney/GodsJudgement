using UnityEngine;

public class EnemyAnimator : MonoBehaviour
{
    private Animator animator;
    private UnitMovement movement;
    private Health health;
    private EnemyBrain brain;
    private bool wasWalking = false;
    private bool wasAttacking = false;

    void Start()
    {
        animator = GetComponent<Animator>();
        movement = GetComponent<UnitMovement>();
        health = GetComponent<Health>();
        brain = GetComponent<EnemyBrain>();
    }

    void Update()
    {
        if (health != null && health.isDead)
        {
            return;
        }
        UpdateMovementAnimation();
        UpdateAttackAnimation();
    }

    void UpdateMovementAnimation()
    {
        bool isWalking = movement != null && !movement.HasReachedTarget() && !wasAttacking;
        if (isWalking != wasWalking)
        {
            animator.SetBool("IsWalking", isWalking);
            wasWalking = isWalking;
        }
    }

    void UpdateAttackAnimation()
    {
        bool isAttacking = brain != null && brain.IsAttacking();

        if (isAttacking != wasAttacking)
        {
            animator.SetBool("IsAttacking", isAttacking);
            wasAttacking = isAttacking;
            print("attacking");
        }
    }

    public void TriggerHit()
    {
        if (animator != null && (health == null || !health.isDead))
        {
            animator.SetTrigger("Hit");
        }
    }

    public void TriggerDeath()
    {
        if (animator != null)
        {
            animator.SetTrigger("Die");
        }
    }
}