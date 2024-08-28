using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DetectionZone : MonoBehaviour
{
    public UnityEvent noColliderRemain; 
    Collider2D col;
    public List<Collider2D> detectedColliders = new List<Collider2D>();
    // Start is called before the first frame update
    void Start()
    {
        col = GetComponent<Collider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        detectedColliders.Add(collision);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        detectedColliders.Remove(collision);
        if (detectedColliders.Count <= 0 ) {
            noColliderRemain.Invoke();
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
