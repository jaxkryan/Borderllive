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
    //public float walkSpeed = 3f;
    Rigidbody2D rb;
    public enum WalkableDirection { Right, Left }
    TouchingDirection touchingDirection;

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
        //Debug.Log("erd " + enemyStat.Endurance);
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
        if (AttackCooldown > 0)
        {
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
        if (!damageable.LockVelocity)
        {
            if (CanMove && touchingDirection.IsGround)
            {
                rb.velocity = new Vector2(enemyStat.Speed * walkDirectionVector.x, rb.velocity.y);
            }
            else rb.velocity = new Vector2(Mathf.Lerp(rb.velocity.x, 0, walkStopRate), rb.velocity.y);
        }
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


    //slow debuff
    public void SlowDown(int id, int duration, float speedReduction)
    {
        Water_1 water_1 = new Water_1();
        // // slow down animations by half
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
        enemyStat.Speed -= enemyStat.BaseSpeed*speedReduction;
        animator.speed -= animator.speed*speedReduction;
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
