
using UnityEngine;

public class SelfDestruct : MonoBehaviour
{
    public float destructionDelay = 5f; // Time in seconds before the GameObject is deactivated

    private void Start()
    {
        // Start the self-destruction countdown
        Invoke("DeactivateGameObject", destructionDelay);
    }

    private void DeactivateGameObject()
    {
        // Deactivate the GameObject
        gameObject.SetActive(false);
    }
}

