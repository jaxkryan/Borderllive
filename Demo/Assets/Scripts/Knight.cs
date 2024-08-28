using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(TouchingDirection), typeof(Damageable))]
public class Knight : MonoBehaviour
{
    public DetectionZone attackZone;
    public DetectionZone cliffDetectionZone;
    public float walkStopRate = 0.05f;
    Animator animator;
    Damageable damageable;
    public float walkSpeed = 3f;
    Rigidbody2D rb;
    public enum WalkableDirection { Right, Left}
    TouchingDirection touchingDirection;

    private Vector2 walkDirectionVector = Vector2.right;
    private WalkableDirection _walkDirection;
    public WalkableDirection WalkDirection { get { return _walkDirection; } set {
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
        } }

    public bool HasTarget { get { return _hasTarget; }
        private set { _hasTarget = value;
            animator.SetBool(AnimationStrings.hasTarget, value); } 
    }

    private void Awake()
    {
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
        HasTarget = attackZone.detectedColliders.Count > 0;
        if (AttackCooldown > 0 ) { 
            AttackCooldown -= Time.deltaTime; 
        }
        
    }

    public bool _hasTarget = false;

    private void FixedUpdate()
    {
        //if (damageable.LockVelocity) // Freeze all movement if stunned
        //{
        //    rb.velocity = Vector2.zero; // Set velocity to zero
        //    return;
        //}

        if (touchingDirection.IsOnWall && touchingDirection.IsGround)
        {
            FlipDirection();
        }
        if(!damageable.LockVelocity)
        {
            if (CanMove && touchingDirection.IsGround)
            {
                rb.velocity = new Vector2(walkSpeed * walkDirectionVector.x, rb.velocity.y);
            }
            else rb.velocity = new Vector2(Mathf.Lerp(rb.velocity.x, 0, walkStopRate), rb.velocity.y);
        }
    }

    private void FlipDirection()
    {
        if(WalkDirection == WalkableDirection.Right)
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
    }
    public bool CanMove
    {
        get {
            return animator.GetBool(AnimationStrings.canMove);
        }
    }

    public float AttackCooldown { get {
            return animator.GetFloat(AnimationStrings.AttackCooldown);
        } private set
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
}
