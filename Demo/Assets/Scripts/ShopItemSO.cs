using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Item ScriptableObject
[CreateAssetMenu(fileName = "shopMenu", menuName = "Scriptable Objects/New Shop Item", order = 1)]
public class ShopItemSO : ScriptableObject
{
    public string title;
    public string description;
    public int baseCost;
}
