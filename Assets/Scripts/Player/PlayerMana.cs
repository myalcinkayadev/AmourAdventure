using UnityEngine;

public class PlayerMana : MonoBehaviour
{
    [Header("Config")]
    [SerializeField] private PlayerStats stats;

    public float CurrentMana => stats.Mana;

    public void ResetMana()
    {
        stats.Mana = stats.MaxMana;
    }

    public void UseMana(float amount)
    {
        stats.Mana = Mathf.Max(stats.Mana - amount, 0f);
    }

    private void Start()
    {
        ResetMana();
    }
}
