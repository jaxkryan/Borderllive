using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingEyeShooting : MonoBehaviour
{
    [SerializeField]
    public GameObject projectilePrefab;
    [SerializeField]
    public Transform projectilePos;
    [SerializeField]
    public float cooldown;
    [SerializeField]
    public float distance;
    private GameObject player;

    private float timer;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        float distance = Vector2.Distance(transform.position, player.transform.position);

        if (distance < cooldown) {
            timer += Time.deltaTime;

            if (timer > cooldown)
            {
                timer = 0;
                ShootProjectile();
            }
        }
    }

    void ShootProjectile()
    {
        Instantiate(projectilePrefab, projectilePos.position, Quaternion.identity);
    }
}
