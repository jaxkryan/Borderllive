using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassthroughPlatform : MonoBehaviour
{
    private Collider2D _collider;
    private bool isPlayerOnPlatform;

    private void Start()
    {
        _collider = GetComponent<Collider2D>();
    }

    private void SetIsPlayerOnPlatform(Collision2D other, bool value)
    {
        var player = other.gameObject.GetComponent<PlayerController>();
        if (player != null)
        {
            isPlayerOnPlatform = value;
        }
    }

    private void Update()
    {
        if (isPlayerOnPlatform && Input.GetAxisRaw("Vertical") < 0)
        {
            _collider.enabled = false;
            StartCoroutine(EnableCollider());
        }
    }

    private IEnumerator EnableCollider()
    {
        yield return new WaitForSeconds(0.5f);
        _collider.enabled = true;

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        SetIsPlayerOnPlatform(collision, true);
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        SetIsPlayerOnPlatform(collision, false);
    }

}
