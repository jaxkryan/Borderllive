using System;
using System.Collections;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(TouchingDirection), typeof(Damageable))]
public class Knight : MonoBehaviour
{
    private EnemyStat enemyStat;
    private OwnedDebuff ownedDebuff;
    public DetectionZone attackZone;
    public DetectionZone cliffDetectionZone;

    public float walkStopRate = 0.05f;
    public float chaseSpeedMultiplier = 1.06f; // Speed multiplier when chasing the player
    public float minChaseDistance = 1.5f;      // Minimum distance to keep from the player
    public float attackDistance = 1.0f;        // Distance to attack the player

    Animator animator;
    Damageable damageable;
    Rigidbody2D rb;
    public enum WalkableDirection { Right, Left }
    TouchingDirection touchingDirection;

    private Vector2 walkDirectionVector = Vector2.right;
    private WalkableDirection _walkDirection;
    private Transform player;

    public WalkableDirection WalkDirection
    {
        get { return _walkDirection; }
        set
        {
            if (_walkDirection != value)
            {
                gameObject.transform.localScale = new Vector2(gameObject.transform.localScale.x * -1, gameObject.transform.localScale.y);
                walkDirectionVector = value == WalkableDirection.Right ? Vector2.right : Vector2.left;
            }
            _walkDirection = value;
        }
    }

    public bool HasTarget
    {
        get { return _hasTarget; }
        private set
        {
            _hasTarget = value;
            animator.SetBool(AnimationStrings.hasTarget, value);
        }
    }

    private bool _hasTarget = false;

    private void Awake()
    {
        enemyStat = GetComponent<EnemyStat>();
        ownedDebuff = GetComponent<OwnedDebuff>();
        rb = GetComponent<Rigidbody2D>();
        touchingDirection = GetComponent<TouchingDirection>();
        animator = GetComponent<Animator>();
        damageable = GetComponent<Damageable>();
    }

    void Start()
    {
    }

    void Update()
    {
        // Check if player is in attack zone
        HasTarget = attackZone.detectedColliders.Count > 0;
        if (HasTarget)
        {
            player = attackZone.detectedColliders.FirstOrDefault(collider => collider.CompareTag("Player"))?.transform;

            // Perform attack if within attack distance
            if (player != null && Vector2.Distance(transform.position, player.position) <= attackDistance)
            {
                Attack(); // Trigger attack via animator
            }
        }

        // Manage attack cooldown
        if (AttackCooldown > 0)
        {
            AttackCooldown -= Time.deltaTime;
        }
    }

    private void FixedUpdate()
    {
        // Update the player reference if there's a target detected
        if (HasTarget)
        {
            player = attackZone.detectedColliders.FirstOrDefault(collider => collider.CompareTag("Player"))?.transform;
        }
        else
        {
            player = null; // Clear player reference if not detected
        }

        // Chase or attack the player based on distance
        if (player != null)
        {
            float distanceToPlayer = Vector2.Distance(transform.position, player.position);

            // Chase the player if they are outside attack distance
            if (distanceToPlayer > attackDistance)
            {
                ChasePlayer(distanceToPlayer);
            }
            else
            {
                rb.velocity = Vector2.zero; // Stop movement if in attack range
            }
        }
        else
        {
            // Handle normal movement if no player detected
            NormalMovement();
        }

        // Cliff detection and wall handling
        if (touchingDirection.IsOnWall && touchingDirection.IsGround)
        {
            FlipDirection();
        }

        // Check if the knight can move
        if (!damageable.LockVelocity)
        {
            if (CanMove && touchingDirection.IsGround)
            {
                rb.velocity = new Vector2(enemyStat.Speed * walkDirectionVector.x, rb.velocity.y);
            }
            else
            {
                rb.velocity = new Vector2(Mathf.Lerp(rb.velocity.x, 0, walkStopRate), rb.velocity.y);
            }
        }
    }


    private void ChasePlayer(float distanceToPlayer)
    {
        if (distanceToPlayer > minChaseDistance)
        {
            Vector2 direction = (player.position - transform.position).normalized;
            rb.velocity = new Vector2(direction.x * enemyStat.Speed * chaseSpeedMultiplier, rb.velocity.y);
            WalkDirection = direction.x > 0 ? WalkableDirection.Right : WalkableDirection.Left;
        }
    }

    private void Attack()
    {
        // Trigger attack animation
        if (AttackCooldown <= 0)
        {
            animator.SetTrigger(AnimationStrings.attacking); // Ensure this matches your animator setup
        }
    }

    private void FlipDirection()
    {
        WalkDirection = WalkDirection == WalkableDirection.Right ? WalkableDirection.Left : WalkableDirection.Right;
    }

    public void OnHit(int damage, Vector2 knockback)
    {
        rb.velocity = new Vector2(knockback.x, rb.velocity.y + knockback.y);
    }

    public bool CanMove
    {
        get
        {
            return animator.GetBool(AnimationStrings.canMove);
        }
    }

    public float AttackCooldown
    {
        get
        {
            return animator.GetFloat(AnimationStrings.AttackCooldown);
        }
        private set
        {
            animator.SetFloat(AnimationStrings.AttackCooldown, Mathf.Max(0, value));
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Enemy"))
        {
            FlipDirection();
        }
    }

    public void OnCliffDetected()
    {
        if (touchingDirection.IsGround)
        {
            FlipDirection();
        }
    }

    internal void ReduceDefense(int id, int duration, float defenseReduction)
    {
        if (ownedDebuff.activeDebuff.Any(debuff => debuff.id == id))
        {
            Debug.Log("Debuff with id " + id + " is already active. Skipping.");
            return;
        }
        if (enemyStat == null)
        {
            Debug.LogError("enemyStat component not found!");
            return;
        }
        float defReducValue = enemyStat.BaseEndurance * defenseReduction;
        Debug.Log("def shred value: " + defReducValue);
        enemyStat.Endurance -= defReducValue;
        Debug.Log("enemy current def: " + enemyStat.Endurance);
        StartCoroutine(RestoreDefenseAfterDuration(duration));
    }

    private IEnumerator RestoreDefenseAfterDuration(float duration)
    {
        yield return new WaitForSeconds(duration);
        enemyStat.Endurance = enemyStat.BaseEndurance;
    }

    // Slow debuff
    public void SlowDown(int id, int duration, float speedReduction)
    {
        if (ownedDebuff.activeDebuff.Any(debuff => debuff.id == id))
        {
            Debug.Log("Debuff with id " + id + " is already active. Skipping.");
            return;
        }
        if (enemyStat == null)
        {
            Debug.LogError("enemyStat component not found!");
            return;
        }
        enemyStat.Speed -= enemyStat.BaseSpeed * speedReduction;
        animator.speed -= animator.speed * speedReduction;
        Debug.Log("slow successfully in " + duration);
        StartCoroutine(RestoreSpeedAfterDuration(duration));
    }

    private IEnumerator RestoreSpeedAfterDuration(float duration)
    {
        yield return new WaitForSeconds(duration);
        animator.speed = 1f;
        enemyStat.Speed = enemyStat.BaseSpeed;
    }

    internal void TakeMoreDmg(float dmgIncrease, float hpThreshold)
    {
        throw new NotImplementedException();
    }

    private void NormalMovement()
    {
        if (touchingDirection.IsGround)
        {
            rb.velocity = new Vector2(enemyStat.Speed * walkDirectionVector.x, rb.velocity.y);
        }
    }
}
