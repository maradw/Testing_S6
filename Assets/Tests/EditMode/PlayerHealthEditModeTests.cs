using NUnit.Framework;
using UnityEngine;

public class PlayerHealthEditModeTests
{
    [Test]
    public void Initialize_SetsValuesAndAlive()
    {
        var go = new GameObject("player");
        var ph = go.AddComponent<PlayerHealth>();

        ph.Initialize(health: 50, armor: 10, maxHealthOverride: 100, maxArmorOverride: 20);

        Assert.AreEqual(100, ph.MaxHealth);
        Assert.AreEqual(20, ph.MaxArmor);
        Assert.AreEqual(50, ph.CurrentHealth);
        Assert.AreEqual(10, ph.CurrentArmor);
        Assert.IsTrue(ph.IsAlive);

        Object.DestroyImmediate(go);
    }

    [Test]
    public void Damage_UsesArmorThenHealth()
    {
        var go = new GameObject("player");
        var ph = go.AddComponent<PlayerHealth>();
        ph.Initialize(health: 100, armor: 20, maxHealthOverride: 100, maxArmorOverride: 20);

        ph.TakeDamage(15);
        Assert.AreEqual(100, ph.CurrentHealth);
        Assert.AreEqual(5, ph.CurrentArmor);

        ph.TakeDamage(10);
        Assert.AreEqual(95, ph.CurrentHealth); // 5 armor left absorbs 5, 5 goes to health
        Assert.AreEqual(0, ph.CurrentArmor);

        Object.DestroyImmediate(go);
    }

    [Test]
    public void Death_FiresOnDiedOnce()
    {
        var go = new GameObject("player");
        var ph = go.AddComponent<PlayerHealth>();
        ph.Initialize(health: 10, armor: 0, maxHealthOverride: 10, maxArmorOverride: 0);
        int diedCount = 0;
        ph.OnDied += () => diedCount++;

        ph.TakeDamage(10);
        Assert.IsFalse(ph.IsAlive);
        Assert.AreEqual(1, diedCount);

        ph.TakeDamage(5); // no effect when dead
        Assert.AreEqual(1, diedCount);

        Object.DestroyImmediate(go);
    }
}
