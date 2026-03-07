using UnityEngine;

public class TroopAnimator : MonoBehaviour
{
    private Animator animator;
    private UnitMovement movement;
    private Health health;
    private UtilityBrain brain;
    private UnitCombat combat;

    void Start()
    {
        animator = GetComponentInChildren<Animator>();
        movement = GetComponent<UnitMovement>();
        health = GetComponent<Health>();
        brain = GetComponent<UtilityBrain>();
        combat = GetComponent<UnitCombat>();
        if (animator != null && combat != null)
        {
            AnimationClip attackClip = GetClipByName("attack");
            if (attackClip != null)
            {
                float desiredSpeed = attackClip.length / combat.attackCooldown;
                animator.SetFloat("AttackSpeed", desiredSpeed);
            }
        }
    }

    void Update()
    {
        if (animator == null)
        {
            return;
        }
        if (health != null && health.isDead)
        {
            return;
        }

        bool isAttacking = combat != null && combat.IsAttacking();
        bool isWalking = movement != null && !movement.HasReachedTarget() && !isAttacking;

        animator.SetBool("IsWalking", isWalking);
        animator.SetBool("IsAttacking", isAttacking);
    }

    public void TriggerHit()
    {
        if (animator != null)
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

    AnimationClip GetClipByName(string name)
    {
        foreach (AnimationClip clip in animator.runtimeAnimatorController.animationClips)
        {
            if (clip.name.ToLower().Contains(name))
            {
                return clip;
            }
        }
        return null;
    }
}