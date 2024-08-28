using UnityEngine;
using UnityEngine.Tilemaps;

public class FloatingPlatformTilemap : MonoBehaviour
{
    private TilemapCollider2D platformCollider;
    private Collider2D playerCollider;

    public string playerTag = "Player";
    public KeyCode fallThroughKey = KeyCode.S;

    private bool playerOnPlatform = false;

    void Start()
    {
        platformCollider = GetComponent<TilemapCollider2D>();
        if (platformCollider == null)
        {
            Debug.LogError("TilemapCollider2D component not found on the platform!");
        }
    }

    void Update()
    {
        if (playerOnPlatform && Input.GetKeyDown(fallThroughKey))
        {
            DisableCollisionTemporarily();
        }
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(playerTag))
        {
            playerCollider = collision.collider;
            Vector2 contactPoint = collision.GetContact(0).point;

            // Check if the player is on top of the platform
            if (contactPoint.y > transform.position.y)
            {
                playerOnPlatform = true;
                Physics2D.IgnoreCollision(platformCollider, playerCollider, false);
            }
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(playerTag))
        {
            playerOnPlatform = false;
            DisableCollisionTemporarily();
        }
    }

    void DisableCollisionTemporarily()
    {
        if (playerCollider != null && gameObject.activeInHierarchy)
        {
            Physics2D.IgnoreCollision(platformCollider, playerCollider, true);
            Invoke("EnableCollision", 0.5f); // Adjust this time as needed
        }
    }

    void EnableCollision()
    {
        if (playerCollider != null)
        {
            Physics2D.IgnoreCollision(platformCollider, playerCollider, false);
        }
    }

    void OnDisable()
    {
        // Ensure we don't leave collisions ignored if the object is disabled
        if (playerCollider != null)
        {
            Physics2D.IgnoreCollision(platformCollider, playerCollider, false);
        }
    }
}