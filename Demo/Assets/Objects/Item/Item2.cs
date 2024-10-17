using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Item2", menuName = "Item/Item2")]

public class Item2 : Item
{
    PlayerController playerController;

    private void OnEnable()
    {
        this.itemName = "DmgAllEnemy";
        this.itemDescription = "Deal true damage to all enemies equal to 20% number of soul. " +
        "If only one enemy is hit, the damage will be equal to 40% number of soul. "
        + "The damage can not exceed the enemy's 40% max health.";
        this.itemType = ItemType.Active;
        this.cd = 20f;
        this.cost = 100;
                this.code = "daikalop12a";
        this.isEnable = false;
        this.historyDescription = "Năm 1601, để chuẩn bị cho quá trình mở rộng bờ cõi, xây dựng giang sơn cơ đồ, chúa Nguyễn Hoàng đã cùng binh lính rong ruổi vó ngựa dọc hai bên bờ sông Hương. Ông bất chợt bắt gặp hình ảnh một ngọn đồi nhỏ nhô lên bên dòng sông xanh biếc, thế tựa con rồng quay đầu nhìn lại. Tương truyền rằng mỗi đêm lại có một bà lão mặc áo đỏ, với khuôn mặt phúc hậu, đi lên đồi Hạ Khuê và nói với mọi người rằng: Tại đây sẽ có một vị chúa lập chùa để trấn giữ long mạch. Thấy ý tưởng của mình có sự tương thông với câu chuyện kể lại, Nguyễn Hoàng ngay lập tức cho quân lính xây dựng ngôi chùa trên đồi.";
        InitializeLocalization("Item2", "Item2_Description", "Item2_HistoryDescription");
        LoadItemState();
    }

    public override void Activate()
    {
         if (isEnable==false) return;
        playerController = FindAnyObjectByType<PlayerController>();
        float numberOfSoul = playerController.GetComponent<CurrencyManager>().currentAmount;
        Debug.Log("Number of soul: " + numberOfSoul);

        GameObject[] allEnemies = GameObject.FindGameObjectsWithTag("Enemy");

        // Calculate the damage based on the number of enemies
        float damageMultiplier = allEnemies.Length == 1 ? 0.4f : 0.2f; // 40% if one enemy, 20% otherwise
        float baseDamage = numberOfSoul * damageMultiplier;
        Debug.Log(baseDamage);
        // Apply damage to each enemy
        foreach (GameObject enemy in allEnemies)
        {
            // Get the Damageable component from the enemy
            Damageable enemyDamageable = enemy.GetComponent<Damageable>();
            EnemyStat enemyStat = enemy.GetComponent<EnemyStat>();
            if (enemyDamageable != null && enemyDamageable.IsAlive)
            {
                // Calculate the damage ensuring it doesn't exceed 40% of the enemy's max health
                float damageToDeal = Mathf.Min((int)baseDamage, (int)(0.4f * enemyDamageable.MaxHealth)) + enemyStat.DEF;
                // Apply the calculated damage
                enemyDamageable.Hit((int)damageToDeal, Vector2.zero); // Assuming no knockback, pass (0,0) for the knockback vector
                Debug.Log($"Dealt {damageToDeal} damage to {enemy.name}");
            }
            else
            {
                Debug.LogWarning($"No Damageable component found on {enemy.name} or enemy is already dead.");
            }
        }
    }
}