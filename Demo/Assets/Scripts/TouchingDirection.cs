using UnityEngine;

public class TouchingDirection : MonoBehaviour
{
    public ContactFilter2D castFilter;
    public float groundDistance = 0.05f;
    private float wallDistance = 0.2f;
    private float cellingDistance = 0.05f;
    CapsuleCollider2D touchingCol;
    RaycastHit2D[] groundHits = new RaycastHit2D[5];
    RaycastHit2D[] wallHits = new RaycastHit2D[5];
    RaycastHit2D[] cellingHits = new RaycastHit2D[5];

    [SerializeField]
    private bool _isGround = true;

    Animator animator;
    public bool IsGround
    {
        get
        {
            return _isGround;
        }
        private set
        {
            _isGround = value;
            animator.SetBool(AnimationStrings.isGrounded, value);
        }
    }
    [SerializeField]
    private bool _isOnWall = false;
    public bool IsOnWall
    {
        get
        {
            return _isOnWall;
        }
        private set
        {
            _isOnWall = value;
            animator.SetBool(AnimationStrings.isOnWall, value);
        }
    }
    [SerializeField]
    private bool _isOnCelling = false;


    private Vector2 wallCheckDirection => gameObject.transform.localScale.x > 0 ? Vector2.right : Vector2.left;

    public bool IsOnCelling
    {
        get
        {
            return _isOnCelling;
        }
        private set
        {
            _isOnCelling = value;
            animator.SetBool(AnimationStrings.isOnCelling, value);
        }
    }


    private void Awake()
    {
        touchingCol = GetComponent<CapsuleCollider2D>();
        animator = GetComponent<Animator>();
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        IsGround = touchingCol.Cast(Vector2.down, castFilter, groundHits, groundDistance) > 0;
        IsOnWall = touchingCol.Cast(wallCheckDirection, castFilter, wallHits, wallDistance) > 0;
        IsOnCelling = touchingCol.Cast(Vector2.up, castFilter, cellingHits, cellingDistance) > 0;
    }
}
