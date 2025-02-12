/// <summary>
/// Defines the functionality for objects that can take damage.
/// </summary>
public interface IDamageable
{
    /// <summary>
    /// Applies the specified amount of damage to the object.
    /// </summary>
    /// <param name="amount">The amount of damage to apply.</param>
    void TakeDamage(float amount);
}
