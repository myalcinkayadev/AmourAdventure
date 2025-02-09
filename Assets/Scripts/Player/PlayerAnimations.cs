using UnityEngine;

public class PlayerAnimations : MonoBehaviour
{
    private readonly int moveXHash = Animator.StringToHash("MoveX");
    private readonly int moveYHash = Animator.StringToHash("MoveY");
    private readonly int isMovingHash = Animator.StringToHash("IsMoving");
    private readonly int isAttackingHash = Animator.StringToHash("IsAttacking");
    private readonly int deadHash = Animator.StringToHash("Dead");
    private readonly int reviveHash = Animator.StringToHash("Revive");

    private Animator animator;

    public void ResetAnimation() {
        SetMoveAnimation(Vector2.down);
        SetReviveAnimation();
    }

    public void SetMoveAnimation(Vector2 moveDirection) {
        bool isPlayerMoving = moveDirection != Vector2.zero;
        animator.SetBool(isMovingHash, isPlayerMoving);
    
        if (isPlayerMoving) {
            animator.SetFloat(moveXHash, moveDirection.x);
            animator.SetFloat(moveYHash, moveDirection.y);
        }
    }

    public void SetDeadAnimation() {
        animator.SetTrigger(deadHash);
    }

    public void SetAttackAnimation(bool value) {
        animator.SetBool(isAttackingHash, value);
    }

    public void SetReviveAnimation() {
        animator.SetTrigger(reviveHash);
    }

    private void Awake() {
        animator = GetComponent<Animator>();
    }
}
