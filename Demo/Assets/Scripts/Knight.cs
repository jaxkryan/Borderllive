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
    Animator animator;
    Damageable damageable;
    Rigidbody2D rb;

    public enum WalkableDirection { Right, Left }
    TouchingDirection touchingDirection;

    private Vector2 walkDirectionVector = Vector2.right;
    private WalkableDirection _walkDirection;

    public DetectionZone chaseZone; // New chase detection zone
    private bool isChasing = false; // Track whether the enemy is chasing
    private Transform target; // Store the target player's transform

    // New variable to track attack state
    private bool isAttacking = false;

    public WalkableDirection WalkDirection
    {
        get { return _walkDirection; }
        set
        {
            if (_walkDirection != value)
            {
                gameObject.transform.localScale =
                    new Vector2(gameObject.transform.localScale.x * -1, gameObject.transform.localScale.y);
                if (value == WalkableDirection.Right)
                {
                    walkDirectionVector = Vector2.right;
                }
                else if (value == WalkableDirection.Left)
                {
                    walkDirectionVector = Vector2.left;
                }
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

    private void Awake()
    {
        enemyStat = GetComponent<EnemyStat>();
        ownedDebuff = GetComponent<OwnedDebuff>();
        rb = GetComponent<Rigidbody2D>();
        touchingDirection = GetComponent<TouchingDirection>();
        animator = GetComponent<Animator>();
        damageable = GetComponent<Damageable>();
    }

    void Update()
    {
        HasTarget = attackZone.detectedColliders.Count > 0;

        // Check if the player is in the chase zone
        Collider2D playerCollider = chaseZone.detectedColliders
            .FirstOrDefault(collider => collider.CompareTag("Player")); // Assuming the player has the tag "Player"

        if (playerCollider != null) // If we found the player collider
        {
            target = playerCollider.transform; // Set the target to the player's transform
            StartChasing();
        }
        else if (isChasing)
        {
            StopChasing();
        }

        // Attack cooldown logic
        if (AttackCooldown > 0)
        {
            AttackCooldown -= Time.deltaTime;
        }
    }

    private void FixedUpdate()
    {
        if (isChasing && !isAttacking) // Check if not attacking
        {
            ChasePlayer();
        }
        else
        {
            // Existing patrol logic...
            if (touchingDirection.IsOnWall && touchingDirection.IsGround)
            {
                FlipDirection();
            }

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
    }

    private void StartChasing()
    {
        isChasing = true;
    }

    private void StopChasing()
    {
        isChasing = false;
        target = null;
    }

    private void ChasePlayer()
    {
        if (target == null) return;

        // Move towards the player's position
        Vector2 direction = (target.position - transform.position).normalized;

        // Check if the player is to the left or right of the enemy
        if (direction.x < 0 && WalkDirection == WalkableDirection.Right)
        {
            FlipDirection(); // Flip to face the player if they are behind
        }
        else if (direction.x > 0 && WalkDirection == WalkableDirection.Left)
        {
            FlipDirection(); // Flip to face the player if they are behind
        }

        // Update velocity to chase the player
        rb.velocity = new Vector2(direction.x * enemyStat.Speed, rb.velocity.y);

        // Check if the enemy can attack
        if (HasTarget && AttackCooldown <= 0)
        {
            AttackPlayer();
        }
    }


    private void AttackPlayer()
    {
        isAttacking = true; // Set the attack state
        animator.SetBool(AnimationStrings.attacking, true); // Trigger attack animation

        // Attack logic...
        AttackCooldown = 1.0f; // Set your desired cooldown duration

        // You may want to reset isAttacking after the attack animation ends.
        StartCoroutine(ResetAttackStateAfterAnimation());
    }

    private IEnumerator ResetAttackStateAfterAnimation()
    {
        // Assuming your attack animation length is defined elsewhere, wait for its duration.
        yield return new WaitForSeconds(1.0f); // Adjust based on your animation duration
        isAttacking = false; // Reset the attacking state
        animator.SetBool(AnimationStrings.attacking, false); // Stop the attack animation
    }

    public bool _hasTarget = false;

    private void FlipDirection()
    {
        if (WalkDirection == WalkableDirection.Right)
        {
            WalkDirection = WalkableDirection.Left;
        }
        else if (WalkDirection == WalkableDirection.Left)
        {
            WalkDirection = WalkableDirection.Right;
        }
        else
        {
            Debug.LogError("ERROR");
        }
    }

    public void OnHit(int damage, Vector2 knockback)
    {
        rb.velocity = new Vector2(knockback.x, rb.velocity.y + knockback.y);
        // Prevent movement when hit, or manage the hit state accordingly
        // If necessary, you can set a separate hit state or delay further actions
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
        Wood_2 wood_2 = new Wood_2();
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
        StartCoroutine(RestoreDefenseAfterDuration(wood_2.duration));
    }

    private IEnumerator RestoreDefenseAfterDuration(float duration)
    {
        yield return new WaitForSeconds(duration);
        enemyStat.Endurance = enemyStat.BaseEndurance;
    }

    // Slow debuff
    public void SlowDown(int id, int duration, float speedReduction)
    {
        Water_1 water_1 = new Water_1();
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
        Debug.Log("slow successfully in " + water_1.duration);
        StartCoroutine(RestoreSpeedAfterDuration(water_1.duration));
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
}
