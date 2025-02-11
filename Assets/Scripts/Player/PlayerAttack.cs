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
    [SerializeField] private LayerMask enemyLayer;

    [Header("Melee Configuration")]
    [SerializeField] private float meleeRange = 2f;
    [SerializeField] private ParticleSystem slashFX;

    [Header("Player Components")]
    [SerializeField] private PlayerMana playerMana;
    [SerializeField] private PlayerAnimations playerAnimations;
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private PlayerStats playerStats;

    private PlayerAction actions;
    public Weapon CurrentWeapon { get; set; }

    private float attackTimer = 0f;
    private float currentAttackRotation = 0f;
    private Transform currentAttackPosition;

    private Vector2 lastNonZeroMoveDirection = Vector2.up;

    private void Awake()
    {
        actions = new PlayerAction();

        if (playerMana == null) playerMana = GetComponent<PlayerMana>();
        if (playerAnimations == null) playerAnimations = GetComponent<PlayerAnimations>();
        if (playerMovement == null) playerMovement = GetComponent<PlayerMovement>();
    }

    private void Start() {
        CurrentWeapon = equippedWeapon;
    }

    private void Update()
    {
        if (attackTimer > 0f) attackTimer -= Time.deltaTime;

        DetermineAttackDirection();
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

    private void OnAttackTriggered(InputAction.CallbackContext ctx)
    {
        Attack();
    }

    private void Attack()
    {
        if (attackTimer > 0f) return;

        attackTimer = attackCooldown;

        Transform spawnPoint = (currentAttackPosition != null && attackPositions.Length > 0)
            ? currentAttackPosition
            : attackPositions[0]; // Default to Up

        if (equippedWeapon.WeaponType == WeaponType.Magic)
        {
            PerformMagicAttack(spawnPoint);
        }
        else if (equippedWeapon.WeaponType == WeaponType.Melee)
        {
            PerformMeleeAttack(spawnPoint);
        }

        playerAnimations.SetAttackAnimation(true);
        StartCoroutine(StopAttackAnimationAfterDelay(0.3f));
    }

    private void PerformMagicAttack(Transform spawnPoint)
    {
        if (playerMana.CurrentMana < equippedWeapon.ManaCost)
        {
            Debug.Log("Not enough playerMana to cast magic!");
            return;
        }

        Quaternion rotation = Quaternion.Euler(0f, 0f, currentAttackRotation);
        Projectile projectile = Instantiate(equippedWeapon.ProjectilePrefab, spawnPoint.position, rotation);
        projectile.Direction = rotation * Vector3.up;
        projectile.Damage = CalculateAttackDamage();

        playerMana.UseMana(equippedWeapon.ManaCost);
    }

    private void PerformMeleeAttack(Transform spawnPoint)
    {
        if (slashFX != null)
        {
            slashFX.transform.position = spawnPoint.position;
            slashFX.Play();
        }

        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(spawnPoint.position, meleeRange, enemyLayer);
        Debug.Log($"HitCount: {hitColliders.Length}");
        
        foreach (Collider2D hit in hitColliders)
        {
            if (hit.TryGetComponent<IDamageable>(out var damageable))
            {
                damageable.TakeDamage(CalculateAttackDamage());
            }
        }
    }

    private float CalculateAttackDamage()
    {
        float totalDamage = playerStats.BaseDamage + CurrentWeapon.Damage;

        float critRoll = Random.Range(0f, 100f);
        if (critRoll <= playerStats.CriticalChance)
        {
            totalDamage *= playerStats.CriticalMultiplier;
        }

        return totalDamage;
    }

    private void DetermineAttackDirection()
    {
        Vector2 moveDir = playerMovement.MoveDirection;
        // If playerMovement is significant, update lastNonZeroMoveDirection.
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