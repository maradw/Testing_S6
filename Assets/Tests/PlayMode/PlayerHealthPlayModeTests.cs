using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
public class PlayerHealthPlayModeTests
{
    [Test]
    public IEnumerator DamageAndHeal_Sequence_WorksInPlayMode()
    {
        var go = new GameObject("player");
        var ph = go.AddComponent<PlayerHealth>();
        ph.Initialize(health: 100, armor: 10, maxHealthOverride: 100, maxArmorOverride: 10);

        // Frame 1
        yield return null;
        ph.TakeDamage(12); // armor 10 -> 0, health -2
        Assert.AreEqual(98, ph.CurrentHealth);
        Assert.AreEqual(0, ph.CurrentArmor);

        // Frame 2
        yield return null;
        ph.Heal(5);
        Assert.AreEqual(100, ph.CurrentHealth);

        Object.Destroy(go);
    }
}
