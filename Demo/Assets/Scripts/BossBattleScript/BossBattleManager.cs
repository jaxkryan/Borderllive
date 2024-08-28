using UnityEngine;
using UnityEngine.UI;
using Cinemachine;
using System.Collections.Generic;

public class BossBattleManager : MonoBehaviour
{
    public GameObject rightBorderExit;
    public Transform bossSpawnPoint;
    public GameObject bossPrefab;
    public Slider bossHealthBar;
    public CinemachineVirtualCamera playerFollowCamera;
    public CinemachineVirtualCamera bossRoomCamera;
    public Canvas bossUICanvas;

    private GameObject currentBoss;
    private Damageable bossDamageable;

    private void Start()
    {
        bossUICanvas.gameObject.SetActive(false);
        rightBorderExit.SetActive(true);
        SwitchToPlayerCamera();
    }

    public void StartBossBattle()
    {
        bossUICanvas.gameObject.SetActive(true);
        SpawnBoss();
        SwitchToBossRoomCamera();
    }

    private void SpawnBoss()
    {
        currentBoss = Instantiate(bossPrefab, bossSpawnPoint.position, Quaternion.identity);
        bossDamageable = currentBoss.GetComponent<Damageable>();
        bossHealthBar.maxValue = bossDamageable.MaxHealth;
        bossHealthBar.value = bossDamageable.Health;
        bossDamageable.damageableDeath.AddListener(OnBossDeath);
        bossDamageable.damageableHit.AddListener(UpdateBossHealthBar);

        // Check if the spawned boss is a FlyingEye and assign waypoints by name
        FlyingEye flyingEye = currentBoss.GetComponent<FlyingEye>();
        if (flyingEye != null)
        {
            // Assign waypoints based on game object names
            flyingEye.waypoints = new List<Transform>
            {
                GameObject.Find("Waypoint1").transform,
                GameObject.Find("Waypoint2").transform,
                GameObject.Find("Waypoint3").transform,
                GameObject.Find("Waypoint4").transform
                // Add more waypoints as needed
            };
        }
    } 

    private void UpdateBossHealthBar(int damage, Vector2 knockback)
    {
        bossHealthBar.value = bossDamageable.Health;
    }

    private void OnBossDeath()
    {
        rightBorderExit.SetActive(false);
        SwitchToPlayerCamera();
        Destroy(currentBoss);
        bossUICanvas.gameObject.SetActive(false);
    }

    private void SwitchToBossRoomCamera()
    {
        playerFollowCamera.Priority = 0;
        bossRoomCamera.Priority = 10;
    }

    private void SwitchToPlayerCamera()
    {
        bossRoomCamera.Priority = 0;
        playerFollowCamera.Priority = 10;
    }
}