using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(TouchingDirection), typeof(Damageable))]
public class Boss : MonoBehaviour
{
    public DetectionZone attackZone;
    public DetectionZone cliffDetectionZone;
    public float walkStopRate = 0.05f;
    public float walkSpeed = 3f;
    public float chaseSpeed = 5f; // Speed when chasing player
    public float detectionRange = 10f; // Range to detect player
    Animator animator;
    Damageable damageable;
    Rigidbody2D rb;
    TouchingDirection touchingDirection;
    public enum WalkableDirection { Right, Left }
    private Vector2 walkDirectionVector = Vector2.right;
    private WalkableDirection _walkDirection;
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
    public bool _hasTarget = false;
    public bool HasTarget
    {
        get { return _hasTarget; }

        private set
        {
            _hasTarget = value;

            animator.SetBool(AnimationStrings.hasTarget, value);
        }
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
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        touchingDirection = GetComponent<TouchingDirection>();
        animator = GetComponent<Animator>();
        damageable = GetComponent<Damageable>();
    }

    void Update()
    {
        UpdateMovement();
        UpdateAnimation();

        HasTarget = attackZone.detectedColliders.Count > 0;

        if (AttackCooldown > 0)
        {

            AttackCooldown -= Time.deltaTime;

        }
    }

    private void UpdateMovement()
    {
        // Chase player if detected
        Transform playerTransform = DetectPlayer();
        if (playerTransform != null)
        {
            Vector2 direction = (playerTransform.position - transform.position).normalized;
            rb.velocity = new Vector2(direction.x * chaseSpeed, rb.velocity.y);

            // Face towards player
            if (direction.x > 0)
                WalkDirection = WalkableDirection.Right;
            else
                WalkDirection = WalkableDirection.Left;
        }
        else
        {
            // Walk in patrol direction if player is not detected
            if (touchingDirection.IsOnWall && touchingDirection.IsGround)
            {
                FlipDirection();
            }
            if (!damageable.LockVelocity)
            {
                rb.velocity = new Vector2(walkSpeed * walkDirectionVector.x, rb.velocity.y);
            }
        }
    }

    private Transform DetectPlayer()
    {
        Collider2D[] detectedColliders = Physics2D.OverlapCircleAll(transform.position, detectionRange);
        foreach (Collider2D collider in detectedColliders)
        {
            if (collider.CompareTag("Player"))
            {
                return collider.transform;
            }
        }
        return null;
    }

    private void UpdateAnimation()
    {
        animator.SetBool(AnimationStrings.hasTarget, rb.velocity.x != 0); // Set animation based on movement
    }

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
}
