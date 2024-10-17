using UnityEngine;

[CreateAssetMenu(fileName = "Item11", menuName = "Item/Item11")]
public class Item11 : Item
{
    private CharacterStat characterStat;
    private Damageable damageable;

    private void OnEnable()
    {
        this.itemName = "Lemongrass essential oil";
        this.itemDescription = "Greatly increase the chance of drop healing item after defeat enemy in 3 second";
        this.itemType = ItemType.Active;
        this.cd = 20;
        this.cost = 250;
        this.code = "daikalop12a";
        this.isEnable = false;
        this.historyDescription = "";
        InitializeLocalization("Item11", "Item11_Description", "Item11_HistoryDescription");
        LoadItemState();
    }

    public override void Activate()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemies)
        {
            Damageable enemyDamageable = enemy.GetComponent<Damageable>();
            if (enemyDamageable != null)
            {
                // Call the coroutine on the enemy's Damageable to boost drop rate for 3 seconds
                enemyDamageable.StartCoroutine(enemyDamageable.BoostDropRate(5f));
            }
            else
            {
                Debug.LogWarning("The enemy does not have a Damageable component.");
            }
        }
    }
}
