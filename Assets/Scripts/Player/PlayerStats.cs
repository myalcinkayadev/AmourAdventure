using UnityEngine;

[CreateAssetMenu(fileName = "PlayerStats", menuName = "Player Stats")]
public class PlayerStats : ScriptableObject
{
    [Header("Level Config")]
    public int Level;
    
    [Header("Health Config")]
    public float Health;
    public float MaxHealth;

    [Header("Mana Config")]
    public float Mana;
    public float MaxMana;

    [Header("Exp Config")]
    public float CurrentExp;
    public float NextLevelExp;
    public float InitialNextLevelExp;
    [Range(1, 100f)]public float ExpMultiplier;

    [Header("Attack Config")]
    public float BaseDamage;
    public float CriticalChance;
    
    [Tooltip("The multiplier applied to damage on a critical hit. For example, if set to 1.5, a critical hit will deal 150% of the base damage.")]
    public float CriticalMultiplier;

    public void ResetStats() {
        Health = MaxHealth;
        Mana = MaxMana;
        Level = 1;
        CurrentExp = 0f;
        NextLevelExp = InitialNextLevelExp;
    }
}
