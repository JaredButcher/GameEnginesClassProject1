using UnityEngine;

public class Damageable : MonoBehaviour
{
    [Tooltip("Multiplier to apply to the received damage")]
    public float damageMultiplier = 1f;
    [Range(0, 1)]
    [Tooltip("Multiplier to apply to self damage")]
    public float sensibilityToSelfdamage = 0.5f;

    public Health health { get; private set; }

    bool isPlayer;
    Difficulty difficulty;

    void Awake()
    {
        // find the health component either at the same level, or higher in the hierarchy
        health = GetComponent<Health>();
        if (!health)
        {
            health = GetComponentInParent<Health>();
        }

        isPlayer = GetComponent<PlayerCharacterController>() != null;
        difficulty = FindObjectOfType<Difficulty>();
    }

    public void InflictDamage(float damage, bool isExplosionDamage, GameObject damageSource)
    {

        if(health)
        {
            var totalDamage = damage;

            // skip the crit multiplier if it's from an explosion
            if (!isExplosionDamage)
            {
                totalDamage *= damageMultiplier;
            }

            // potentially reduce damages if inflicted by self
            if (health.gameObject == damageSource)
            {
                totalDamage *= sensibilityToSelfdamage;
            }

            if (difficulty) {
                if (isPlayer) {
                    totalDamage *= difficulty.GetDifficulty().enemyDamage;
                } else {
                    totalDamage *= difficulty.GetDifficulty().damage;
                }
            }

            // apply the damages
            health.TakeDamage(totalDamage, damageSource);
        }
    }
}
