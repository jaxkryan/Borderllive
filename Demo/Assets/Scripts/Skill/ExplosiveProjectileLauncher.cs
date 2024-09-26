using UnityEngine;

public class ExplosiveProjectileLauncher : MonoBehaviour
{
    public GameObject explosiveProjectilePrefab; // Prefab for the explosive projectile
    public Transform startPoint; // Reference to the Start Point GameObject
    public Transform endPoint; // Reference to the End Point GameObject
    public string skillTag; // Tag for this skill

    public SpellCooldown spellCooldown; // Reference to the SpellCooldown script
    private CharacterStat characterStat; // Reference to the player's CharacterStat

    private void Awake()
    {
        characterStat = GetComponent<CharacterStat>();
    }

    void Start()
    {
        GameObject cooldownObject = GameObject.FindWithTag(skillTag);
        if (cooldownObject != null)
        {
            spellCooldown = cooldownObject.GetComponent<SpellCooldown>();
        }
    }

    public void FireExplosiveProjectile()
    {
        if (spellCooldown != null && spellCooldown.UseSpell()) // Check if the spell can be used
        {
            BerserkGauge berserkGauge = FindAnyObjectByType<BerserkGauge>();

            GameObject explosiveProjectileInstance = Instantiate(explosiveProjectilePrefab, startPoint.position, Quaternion.identity);
            ExplosiveProjectile explosiveProjectile = explosiveProjectileInstance.GetComponent<ExplosiveProjectile>();

            if (explosiveProjectile != null)
            {
                // Set the end point for the projectile
                explosiveProjectile.endPoint = endPoint.position;

                // Set additional damage based on character stats
                explosiveProjectile.SetAdditionalDamage((int)(characterStat.Damage * 1.6));
            }
            if (berserkGauge._isBerserkActive)
            {
                berserkGauge.DecreaseProgress(10f);
            }
            else
            {
                berserkGauge.DecreaseProgress(30f);
            }
        }
    }
}
