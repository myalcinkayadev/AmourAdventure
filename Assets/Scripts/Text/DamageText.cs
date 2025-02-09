using TMPro;
using UnityEngine;

public class DamageText : MonoBehaviour
{
    [Header("Config")]
    [SerializeField] private TextMeshProUGUI damageTMP;

    private void Awake()
    {
        gameObject.SetActive(false);
    }

    public void SetDamageText(float damage) {
        damageTMP.text = damage.ToString("F0");
        gameObject.SetActive(true);
    }

    public void DestroyText() {
        Destroy(gameObject);
    }
}
