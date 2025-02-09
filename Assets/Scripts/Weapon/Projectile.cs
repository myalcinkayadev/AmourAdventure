using UnityEngine;

public class Projectile : MonoBehaviour
{
    [Header("Config")]
    [SerializeField] private float speed = 5f;
    [SerializeField] private float lifetime = 5f;

    public Vector3 Direction { get; set; }
    public float Damage { get; set; }

    private void Start() {
        Destroy(gameObject, lifetime);
    }

    private void Update() {
        transform.Translate(speed * Time.deltaTime * Direction, Space.World);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent<IDamageable>(out var damageable))
        {
            damageable.TakeDamage(Damage);
        }
        Destroy(gameObject);
    }
}
