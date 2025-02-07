using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("Player Stats")]
    [SerializeField] private PlayerStats stats;

    [Header("Bars")]
    [SerializeField] private Image healthBar;
    [SerializeField] private Image manaBar;
    [SerializeField] private Image expBar;

    [Header("Text Elements")]
    [SerializeField] private TextMeshProUGUI levelTMP;
    [SerializeField] private TextMeshProUGUI healthTMP;
    [SerializeField] private TextMeshProUGUI manaTMP;
    [SerializeField] private TextMeshProUGUI expTMP;

    [Header("UI Update Settings")]
    [SerializeField] private float lerpSpeed = 10f; // Speed at which bars smooth out transitions.
    
    private void LateUpdate()
    {
        UpdateBars();
        UpdateText();
    }

    private void UpdateBars()
    {
        healthBar.fillAmount = Mathf.Lerp(healthBar.fillAmount, stats.Health / stats.MaxHealth, lerpSpeed * Time.deltaTime);
        manaBar.fillAmount = Mathf.Lerp(manaBar.fillAmount, stats.Mana / stats.MaxMana, lerpSpeed * Time.deltaTime);
        expBar.fillAmount = Mathf.Lerp(expBar.fillAmount, stats.CurrentExp / stats.NextLevelExp, lerpSpeed * Time.deltaTime);
    }

    private void UpdateText()
    {
        levelTMP.text = $"Level {stats.Level}";
        healthTMP.text = $"{stats.Health} / {stats.MaxHealth}";
        manaTMP.text = $"{stats.Mana} / {stats.MaxMana}";
        expTMP.text = $"{stats.CurrentExp} / {stats.NextLevelExp}";
    }
}
