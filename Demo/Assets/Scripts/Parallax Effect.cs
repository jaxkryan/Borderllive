using UnityEngine;

public class ParallaxEffect : MonoBehaviour
{
    public Camera cam;
    public Transform followTarget;
    Vector2 startPosition;
    float startZ;

    Vector2 camMoveSinceStart => (Vector2)cam.transform.position - startPosition;
    float zDistanceFromTarget => transform.position.z - followTarget.transform.position.z;

    float clippingPlane => (cam.transform.position.z + (zDistanceFromTarget > 0 ? cam.farClipPlane : cam.nearClipPlane));
    float parallaxFactor => Mathf.Abs(zDistanceFromTarget) / clippingPlane;
    // Start is called before the first frame update
    void Start()
    {
        startPosition = transform.position;
        startZ = transform.position.z;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 newPosition = startPosition + camMoveSinceStart * parallaxFactor;
        transform.position = new Vector3(newPosition.x, newPosition.y, startZ);
    }
}
