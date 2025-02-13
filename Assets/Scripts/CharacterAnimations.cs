using UnityEngine;

public abstract class CharacterAnimations : MonoBehaviour
{
    protected Animator animator;

    protected readonly int moveXHash = Animator.StringToHash("MoveX");
    protected readonly int moveYHash = Animator.StringToHash("MoveY");
    protected readonly int isMovingHash = Animator.StringToHash("IsMoving");
    protected readonly int isAttackingHash = Animator.StringToHash("IsAttacking");
    protected readonly int deadHash = Animator.StringToHash("Dead");
    protected readonly int reviveHash = Animator.StringToHash("Revive");

    protected virtual void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void SetMoveAnimation(Vector2 moveDirection)
    {
        bool isMoving = moveDirection != Vector2.zero;
        animator.SetBool(isMovingHash, isMoving);

        if (isMoving)
        {
            animator.SetFloat(moveXHash, moveDirection.x);
            animator.SetFloat(moveYHash, moveDirection.y);
        }
    }

    public void SetDeadAnimation()
    {
        animator.SetTrigger(deadHash);
    }

    public void SetAttackAnimation(bool value)
    {
        animator.SetBool(isAttackingHash, value);
    }

    public void SetReviveAnimation()
    {
        animator.SetTrigger(reviveHash);
    }
}
