using UnityEngine;

public class BossTrigger : MonoBehaviour
{
    private BossBattleManager bossBattleManager;
    private Collider2D triggerCollider;
    private Transform playerTransform;
    private bool playerEnteredFromLeft = false;
    private bool playerExitedToRight = false;

    private void Start()
    {
        bossBattleManager = GetComponentInParent<BossBattleManager>();
        triggerCollider = GetComponent<Collider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerTransform = collision.transform;
            Vector3 playerPosition = playerTransform.position;
            Vector3 triggerPosition = transform.position;

            // Check if player entered from the left
            if (playerPosition.x < triggerPosition.x)
            {
                playerEnteredFromLeft = true;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Vector3 playerPosition = playerTransform.position;
            Vector3 triggerPosition = transform.position;

            // Check if player exited to the right
            if (playerPosition.x > triggerPosition.x && playerEnteredFromLeft)
            {
                playerExitedToRight = true;
            }

            // If player entered from left and exited to right, start boss battle
            if (playerEnteredFromLeft && playerExitedToRight)
            {
                bossBattleManager.StartBossBattle();
                triggerCollider.isTrigger = false; // Turn off the trigger for the collider
            }
        }
    }
}
