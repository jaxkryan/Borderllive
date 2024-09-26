using UnityEngine;

public class DestroyAfterTime : MonoBehaviour
{
    public float lifetime = 1f; // Duration before the explosion effect is destroyed

    private void Start()
    {
        Destroy(gameObject, lifetime);  // Destroy this game object after the specified lifetime
    }
}
