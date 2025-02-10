using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float walkSpeed = 6f;
    [SerializeField] private float runSpeed = 9f;

    private Player player;
    private PlayerAction actions;
    private Rigidbody2D rb2D;
    private PlayerAnimations playerAnimations;

    private Vector2 moveDirection;
    public Vector2 MoveDirection => moveDirection;
    private bool IsDead => player.Stats.Health <= 0;

    private bool isRunning = false;

    private void Awake()
    {
        player = GetComponent<Player>();
        actions = new PlayerAction();
        rb2D = GetComponent<Rigidbody2D>();
        playerAnimations = GetComponent<PlayerAnimations>();
    }

    private void OnEnable()
    {
        actions.Enable();
        actions.Movement.Run.performed += OnRunPerformed;
        actions.Movement.Run.canceled += OnRunCanceled;
    }

    private void OnDisable()
    {
        actions.Movement.Run.performed -= OnRunPerformed;
        actions.Movement.Run.canceled -= OnRunCanceled;
        actions.Disable();
    }

    private void Update()
    {
        ReadMovementInput();
        playerAnimations.SetMoveAnimation(moveDirection);
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        if (IsDead) return;

        float currentSpeed = isRunning ? runSpeed : walkSpeed;
        rb2D.MovePosition(rb2D.position + moveDirection * (currentSpeed * Time.fixedDeltaTime));
    }

    private void ReadMovementInput()
    {
        moveDirection = actions.Movement.Move.ReadValue<Vector2>().normalized;
    }

    private void OnRunPerformed(InputAction.CallbackContext context)
    {
        isRunning = true;
    }

    private void OnRunCanceled(InputAction.CallbackContext context)
    {
        isRunning = false;
    }
}
