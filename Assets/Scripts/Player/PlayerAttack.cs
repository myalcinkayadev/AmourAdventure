using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack : MonoBehaviour
{
    [Header("Weapon Settings")]
    [SerializeField] private Weapon equippedWeapon;

    [Header("Attack Positions (Order: Up, Right, Down, Left)")]
    [SerializeField] private Transform[] attackPositions;

    [Header("Attack Configuration")]
    [SerializeField] private float attackCooldown = 0.5f;
    [SerializeField] private float meleeRange = 2f;
    [SerializeField] private LayerMask enemyLayer;

    [Header("Player Components")]
    [SerializeField] private PlayerMana playerMana;
    [SerializeField] private PlayerAnimations playerAnimations;
    [SerializeField] private PlayerMovement playerMovement;

    private float attackTimer = 0f;
    private float currentAttackRotation = 0f;
    private Transform currentAttackPosition;

    private PlayerAction actions;
    private Collider[] meleeHitBuffer = new Collider[10];

    private Vector2 lastNonZeroMoveDirection = Vector2.up;

    private void Awake()
    {
        actions = new PlayerAction();

        if (playerMana == null) playerMana = GetComponent<PlayerMana>();
        if (playerAnimations == null) playerAnimations = GetComponent<PlayerAnimations>();
        if (playerMovement == null) playerMovement = GetComponent<PlayerMovement>();
    }

    private void OnEnable()
    {
        actions.Enable();
        actions.Attack.TriggerAttack.performed += OnAttackTriggered;
    }

    private void OnDisable()
    {
        actions.Attack.TriggerAttack.performed -= OnAttackTriggered;
        actions.Disable();
    }

    private void Update()
    {
        if (attackTimer > 0f) attackTimer -= Time.deltaTime;

        DetermineAttackDirection();
    }

    private void OnAttackTriggered(InputAction.CallbackContext ctx)
    {
        Attack();
    }

    private void Attack()
    {
        if (attackTimer > 0f)
            return;

        attackTimer = attackCooldown;

        if (equippedWeapon.WeaponType == WeaponType.Magic)
        {
            if (playerMana.CurrentMana < equippedWeapon.ManaCost)
            {
                Debug.Log("Not enough mana to cast magic!");
                return;
            }
            FireProjectile();
            playerMana.UseMana(equippedWeapon.ManaCost);
        }
        else if (equippedWeapon.WeaponType == WeaponType.Melee)
        {
            PerformMeleeAttack();
        }

        playerAnimations.SetAttackAnimation(true);
        StartCoroutine(StopAttackAnimationAfterDelay(0.3f));
    }

    private void FireProjectile()
    {
        Transform spawnPoint = currentAttackPosition;
        if (spawnPoint == null)
        {
            if (attackPositions != null && attackPositions.Length > 0) spawnPoint = attackPositions[0]; // Default to Up.
            else
            {
                Debug.LogError("No attack positions assigned!");
                return;
            }
        }

        Quaternion rotation = Quaternion.Euler(0f, 0f, currentAttackRotation);
        Projectile projectile = Instantiate(equippedWeapon.ProjectilePrefab, spawnPoint.position, rotation);
        projectile.Direction = rotation * Vector3.up;
        projectile.Damage = equippedWeapon.Damage;
    }

    private void PerformMeleeAttack()
    {
        int hitCount = Physics.OverlapSphereNonAlloc(transform.position, meleeRange, meleeHitBuffer, enemyLayer);
        for (int i = 0; i < hitCount; i++)
        {
            IDamageable damageable = meleeHitBuffer[i].GetComponent<IDamageable>();
            if (damageable != null) damageable.TakeDamage(equippedWeapon.Damage);
        }
    }

    private void DetermineAttackDirection()
    {
        Vector2 moveDir = playerMovement.MoveDirection;
        // If movement is significant, update lastNonZeroMoveDirection.
        if (moveDir.sqrMagnitude >= 0.01f) lastNonZeroMoveDirection = moveDir;
        // If idle, use the last nonzero move direction.
        else moveDir = lastNonZeroMoveDirection;

        // Determine rotation and attack position using switch with pattern matching.
        switch (moveDir)
        {
            case Vector2 m when Mathf.Abs(m.x) >= Mathf.Abs(m.y) && m.x > 0f:
                currentAttackRotation = -90f;
                currentAttackPosition = (attackPositions != null && attackPositions.Length > 1) ? attackPositions[1] : null;
                break;
            case Vector2 m when Mathf.Abs(m.x) >= Mathf.Abs(m.y) && m.x < 0f:
                currentAttackRotation = 90f;
                currentAttackPosition = (attackPositions != null && attackPositions.Length > 3) ? attackPositions[3] : null;
                break;
            case Vector2 m when Mathf.Abs(m.y) > Mathf.Abs(m.x) && m.y > 0f:
                currentAttackRotation = 0f;
                currentAttackPosition = (attackPositions != null && attackPositions.Length > 0) ? attackPositions[0] : null;
                break;
            case Vector2 m when Mathf.Abs(m.y) > Mathf.Abs(m.x) && m.y < 0f:
                currentAttackRotation = 180f;
                currentAttackPosition = (attackPositions != null && attackPositions.Length > 2) ? attackPositions[2] : null;
                break;
            default:
                currentAttackRotation = 0f;
                currentAttackPosition = (attackPositions != null && attackPositions.Length > 0) ? attackPositions[0] : null;
                break;
        }
    }

    private IEnumerator StopAttackAnimationAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        playerAnimations.SetAttackAnimation(false);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, meleeRange);

        if (currentAttackPosition != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(currentAttackPosition.position, 0.2f);
        }
    }
}