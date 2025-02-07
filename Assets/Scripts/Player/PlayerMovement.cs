using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Player Config")]
    [SerializeField]
    private float speed = 5f;

    private Player player;
    private PlayerAction actions;
    private Rigidbody2D rb2D;
    private PlayerAnimations playerAnimations;

    private Vector2 moveDirection;

    /// <summary>
    /// Returns whether the player is dead.
    /// </summary>
    private bool IsDead => player.Stats.Health <= 0;

    private void Awake() {
        player = GetComponent<Player>();
        actions = new PlayerAction();
        rb2D = GetComponent<Rigidbody2D>();
        playerAnimations = GetComponent<PlayerAnimations>();
    }


    private void Update()
    {
        ReadMovementInput();
        playerAnimations.UpdateAnimation(moveDirection);
    }

    private void FixedUpdate() {
        Move();
    }

    private void Move() {
        if (IsDead) return;

        rb2D.MovePosition(rb2D.position + moveDirection * (speed * Time.fixedDeltaTime));
    }

    private void ReadMovementInput() {
        moveDirection = actions.Movement.Move.ReadValue<Vector2>().normalized;
    }

    private void OnEnable() {
        actions.Enable();
    }
    private void OnDisable() {
        actions.Disable();
    }

}
