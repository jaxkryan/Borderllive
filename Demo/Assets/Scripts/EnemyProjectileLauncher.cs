using UnityEngine;
using System.Collections;

public class EnemyProjectileLauncher : MonoBehaviour
{
    public Transform launchPoint;
    public GameObject projectilePrefab;
    public GameObject warningLinePrefab; // Reference to the warning line prefab
    public float warningDuration = 1f;   // Duration the warning will be shown before firing
    public bool isVerticalShooter = false; // Set to true for vertical shooting (up/down), false for horizontal

    public bool isLeftShooter = false; // Set to true for left shooter, false for right shooter

    public float minSpeed = 3f;  // Minimum speed for projectile
    public float maxSpeed = 7.5f; // Maximum speed for projectile

    public void FireProjectile()
    {
        StartCoroutine(ShowWarningAndShoot());
    }

    private IEnumerator ShowWarningAndShoot()
    {
        // Step 1: Show the warning line
        GameObject warningLineInstance = Instantiate(warningLinePrefab, launchPoint.position, Quaternion.identity);
        warningLineInstance.transform.SetParent(transform);

        // Adjust warning line's orientation based on whether it's a horizontal or vertical shooter
        if (isVerticalShooter)
        {
            // For vertical shooters, rotate the line to face up/down
            warningLineInstance.transform.rotation = Quaternion.Euler(0, 0, 90); // Rotate for vertical direction
        }

        yield return new WaitForSeconds(warningDuration);

        // Step 2: Destroy the warning line
        Destroy(warningLineInstance);

        // Step 3: Fire the projectile in the specified direction (horizontal or vertical)
        GameObject projectileInstance = Instantiate(projectilePrefab, launchPoint.position, Quaternion.identity);

        EnemyProjectile projectile = projectileInstance.GetComponent<EnemyProjectile>();

        if (projectile != null)
        {
            // Step 4: Determine speed for the projectile
            float speed;
            if (Random.value < 0.05f) // 5% chance
            {
                speed = 20f; // Extremely slow speed
            }
            else
            {
                speed = UnityEngine.Random.Range(minSpeed, maxSpeed);
                speed = Mathf.Min(speed * 10, 85); // Set the speed, ensuring it does not exceed 85
            }
            projectile.speed = speed;

            // Step 5: Set direction based on whether it's vertical or horizontal
            if (isVerticalShooter)
            {
                projectile.direction = Vector2.down; // Shoot downward (for top-to-bottom shooting)
            }
            else
            {
                // Shoot left for left shooter, right for right shooter
                projectile.direction = isLeftShooter ? Vector2.left : Vector2.right;
            }
        }
    }
}
