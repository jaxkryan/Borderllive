using UnityEngine;

public class ProjectileLauncher : MonoBehaviour
{
    public Transform launchPoint;
    public GameObject projectilePrefab;
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

    public void FireProjectile()
    {
        if (spellCooldown != null && spellCooldown.UseSpell()) // Check if the spell can be used
        {
            BerserkGauge berserkGauge = FindAnyObjectByType<BerserkGauge>();

            GameObject projectileInstance = Instantiate(projectilePrefab, launchPoint.position, projectilePrefab.transform.rotation);
            Vector3 origScale = projectileInstance.transform.localScale;
            Projectile projectile = projectileInstance.GetComponent<Projectile>();
            if (projectile != null && characterStat != null)
            {
                projectile.SetAdditionalDamage((int)characterStat.Damage); // Set the CharacterStat on the projectile
            }
            projectileInstance.transform.localScale = new Vector3(
                origScale.x * (transform.localScale.x > 0 ? 1f : -1f),
                origScale.y,
                origScale.z
            );
            if (berserkGauge._isBerserkActive) {
                berserkGauge.DecreaseProgress(10f);
            } else
            {
                berserkGauge.DecreaseProgress(30f);
            }
        }
    }
}
