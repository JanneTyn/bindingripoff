/// <summary>
/// Interface for things that can take damage
/// </summary>
public interface IDamageable
{
    /// <summary>
    /// Damage the object represented by this interface
    /// </summary>
    /// <param name="damageAmount">Amount of damage to take</param>
    public void TakeDamage(float damageAmount);
}
