using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingEyeShooting : MonoBehaviour
{
    [SerializeField] public GameObject projectilePrefab;
    [SerializeField] public Transform projectilePos;
    [SerializeField] public float cooldown;
    [SerializeField] public float distance;
    public DetectionZone shootingDetectionZone; // Detection zone for shooting
    private GameObject player;

    private float timer;


    public bool _hasTarget = false;
    public bool HasTarget
    {
        get { return _hasTarget; }
        private set
        {
            _hasTarget = value;
            animator.SetBool(AnimationStrings.hasShootingTarget, value);
        }
    }

    private void Awake()
    {

        animator = GetComponent<Animator>();
    }
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    Animator animator;
    // Update is called once per frame
    void Update()
    {
        float playerDistance = Vector2.Distance(transform.position, player.transform.position);

        timer += Time.deltaTime;

        // Check if the player is within the detection zone
        HasTarget = shootingDetectionZone.detectedColliders.Count > 0;
        if (timer > cooldown)
        {

            if (HasTarget)
            {
                timer = 0;
            }
        }
    }

    void ShootProjectile()
    {
        HasTarget = false;
        // Instantiate the projectile
        GameObject projectile = Instantiate(projectilePrefab, projectilePos.position, Quaternion.identity);

        // Calculate direction to the player
        Vector2 direction = (player.transform.position - projectilePos.position).normalized;

        // Set the direction for the projectile using the EnemyProjectile script
        EnemyProjectile enemyProjectile = projectile.GetComponent<EnemyProjectile>();
        if (enemyProjectile != null)
        {
            enemyProjectile.SetDirection(direction);
        }
    }
}
