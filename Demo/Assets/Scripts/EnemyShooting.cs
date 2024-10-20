using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

    // New variable to control shooting behavior
    [SerializeField] private bool isStraightShooter = false;

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
        Collider2D playerCollider = shootingDetectionZone.detectedColliders.FirstOrDefault(collider => collider.CompareTag("Player"));
        HasTarget = playerCollider != null; // True if player is found in detected colliders

        // Check cooldown before shooting
        if (timer > cooldown)
        {
            if (HasTarget)
            {
                // Reset the timer when a player is detected and cooldown is met
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
        Vector2 direction;

        if (isStraightShooter)
        {
            // Determine if the player is to the left or right of the enemy
            if (player.transform.position.x > transform.position.x)
            {
                direction = Vector2.right; // Player is to the right
            }
            else
            {
                direction = Vector2.left; // Player is to the left
            }
        }
        else
        {
            // Calculate direction to the player
            direction = (player.transform.position - projectilePos.position).normalized;
        }

        // Set the direction for the projectile using the EnemyProjectile script
        EnemyProjectile enemyProjectile = projectile.GetComponent<EnemyProjectile>();
        if (enemyProjectile != null)
        {
            enemyProjectile.SetDirection(direction);
        }

        // Rotate the projectile to face the direction it is moving (if not straight shooter)
        if (!isStraightShooter)
        {
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            projectile.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        }
        else
        {
            // Set the projectile to face straight right if it's facing right, otherwise set it to face left
            float angle = direction == Vector2.right ? 0 : 180; // 0 degrees for right, 180 for left
            projectile.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        }
    }

}
