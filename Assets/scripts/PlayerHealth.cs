using UnityEngine;

/// <summary>
/// Simple PlayerHealth component with Health + Armor and basic damage/heal logic.
/// Armor absorbs damage first; leftover damage reduces health. Triggers events on damage and death.
/// </summary>
public class PlayerHealth : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private int maxArmor = 0;

    public int MaxHealth => maxHealth;
    public int MaxArmor => maxArmor;
    public int CurrentHealth { get; private set; }
    public int CurrentArmor { get; private set; }
    public bool IsAlive { get; private set; } = true;

    // Events
    public System.Action<int, int, int> OnDamaged; // (damageApplied, currentHealth, currentArmor)
    public System.Action OnDied;

    private void Awake()
    {
        ResetStats();
    }

    /// <summary>
    /// Initialize stats explicitly (helpful for tests).
    /// </summary>
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
        if (CurrentHealth <= 0)
        {
            CurrentHealth = maxHealth;
        }
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

        // Armor absorbs first
        if (CurrentArmor > 0)
        {
            int absorbed = Mathf.Min(CurrentArmor, remaining);
            CurrentArmor -= absorbed;
            remaining -= absorbed;
        }

        // Health takes leftover damage
        if (remaining > 0)
        {
            CurrentHealth = Mathf.Max(0, CurrentHealth - remaining);
        }

        OnDamaged?.Invoke(amount, CurrentHealth, CurrentArmor);

        if (CurrentHealth <= 0 && IsAlive)
        {
            IsAlive = false;
            OnDied?.Invoke();
        }
    }
}
