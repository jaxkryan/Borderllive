using UnityEngine;
using System.Collections;

public class WeaponSelection : MonoBehaviour
{
    public GameObject sword; // Sword GameObject
    public GameObject staff; // Staff GameObject

    private void Start()
    {

        // Check if the player has unlocked the sword and adjust the GameObjects accordingly
        CheckSwordUnlock();
    }

    public void CheckSwordUnlock()
    {
        // Check if the player has unlocked the sword
        if (PlayerPrefs.HasKey("ItemState_sword"))
        {
            // Sword is unlocked, enable both Sword and Staff
            sword.SetActive(true);
            staff.SetActive(true);
        }
        else
        {
            // Sword is not unlocked, disable both Sword and Staff
            sword.SetActive(false);
            staff.SetActive(false);

        }
    }
}
