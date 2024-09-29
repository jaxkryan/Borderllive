using UnityEngine;
using System.Collections;
using Unity.Mathematics;


public class EnemyProjectileLauncher : MonoBehaviour
{
    public Transform launchPoint;
    public GameObject projectilePrefab;
    public GameObject warningLinePrefab; // Reference to the warning line prefab
    public float warningDuration = 1f;   // Duration the warning will be shown before firing
    public bool isVerticalShooter = false; // Set to true for vertical shooting (up/down), false for horizontal

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
        projectile.speed = UnityEngine.Random.Range(2.8f, 8.5f) * 10;
        if (projectile != null)
        {
            // Set direction based on whether it's vertical or horizontal
            if (isVerticalShooter)
            {
                projectile.direction = Vector2.down; // Shoot downward (for top-to-bottom shooting)
            }
            else
            {
                projectile.direction = Vector2.right * (transform.localScale.x > 0 ? 1f : -1f); // Shoot horizontally
            }
        }
    }
}
