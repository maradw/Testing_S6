#if false
// This duplicate script was inside Assets/Tests and caused Unity to treat 'PlayerHealth' as an Editor-only script.
// It is intentionally disabled to avoid conflicts. Keep the real component only in Assets/Scripts (runtime assembly).
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private int maxArmor = 0;

    public int MaxHealth => maxHealth;
    public int MaxArmor => maxArmor;
    public int CurrentHealth { get; private set; }
    public int CurrentArmor { get; private set; }
    public bool IsAlive { get; private set; } = true;

    private void Awake() => ResetStats();
    public void Initialize(int health, int armor, int? maxHealthOverride = null, int? maxArmorOverride = null)
    {
        if (maxHealthOverride.HasValue) maxHealth = Mathf.Max(1, maxHealthOverride.Value);
        if (maxArmorOverride.HasValue) maxArmor = Mathf.Max(0, maxArmorOverride.Value);
        CurrentHealth = Mathf.Clamp(health, 0, maxHealth);
        CurrentArmor = Mathf.Clamp(armor, 0, maxArmor);
        IsAlive = CurrentHealth > 0;
    }
    public void ResetStats()
    {
        CurrentHealth = Mathf.Clamp(CurrentHealth <= 0 ? maxHealth : CurrentHealth, 0, maxHealth);
        CurrentArmor = Mathf.Clamp(CurrentArmor, 0, maxArmor);
        if (CurrentHealth <= 0) CurrentHealth = maxHealth;
        IsAlive = true;
    }
    public void SetMaxHealth(int value, bool refill = true)
    {
        maxHealth = Mathf.Max(1, value);
        if (refill) CurrentHealth = maxHealth; else CurrentHealth = Mathf.Clamp(CurrentHealth, 0, maxHealth);
    }
    public void SetMaxArmor(int value, bool refill = true)
    {
        maxArmor = Mathf.Max(0, value);
        if (refill) CurrentArmor = maxArmor; else CurrentArmor = Mathf.Clamp(CurrentArmor, 0, maxArmor);
    }
    public void AddArmor(int amount)
    {
        if (amount <= 0) return;
        CurrentArmor = Mathf.Clamp(CurrentArmor + amount, 0, maxArmor);
    }
    public void Heal(int amount)
    {
        if (!IsAlive || amount <= 0) return;
        CurrentHealth = Mathf.Clamp(CurrentHealth + amount, 0, maxHealth);
    }
    public void TakeDamage(int amount)
    {
        if (!IsAlive || amount <= 0) return;
        int remaining = amount;
        if (CurrentArmor > 0)
        {
            int absorbed = Mathf.Min(CurrentArmor, remaining);
            CurrentArmor -= absorbed;
            remaining -= absorbed;
        }
        if (remaining > 0) CurrentHealth = Mathf.Max(0, CurrentHealth - remaining);
        if (CurrentHealth <= 0 && IsAlive) IsAlive = false;
    }
}
#endif
