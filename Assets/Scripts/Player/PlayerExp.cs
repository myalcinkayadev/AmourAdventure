using UnityEngine;

/// <summary>
/// Handles player experience management including adding experience and leveling up.
/// </summary>
public class PlayerExp : MonoBehaviour
{
    [Header("Config")]
    [SerializeField] private PlayerStats stats;

    private void Update() {
        if (Input.GetKeyDown(KeyCode.X)) {
            AddExp(300f);
        }
    }

    /// <summary>
    /// Adds the specified amount of experience to the player.
    /// If the current experience exceeds the threshold for leveling up,
    /// the player will level up until the remaining experience is below that threshold.
    /// </summary>
    /// <param name="experienceGained">The amount of experience to add.</param>
    public void AddExp(float experienceGained) {
        stats.CurrentExp += experienceGained;

        while (stats.CurrentExp >= stats.NextLevelExp) {
            stats.CurrentExp -= stats.NextLevelExp;
            LevelUp();
        }
    }

    private void LevelUp() {
        stats.Level++;

        float previousLevelExp = stats.NextLevelExp;
        float newLevelExp = previousLevelExp * (stats.ExpMultiplier / 100f);
        stats.NextLevelExp = Mathf.Round(previousLevelExp + newLevelExp);
    }
}
