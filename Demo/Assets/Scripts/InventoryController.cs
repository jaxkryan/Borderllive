using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Inventory
{
    public class InventoryController : MonoBehaviour
    {
        [SerializeField]
        private UIInventoryPage inventoryUI;

        [SerializeField]
        private InventorySO inventoryData;

        public List<InventoryItem> initialItems = new List<InventoryItem>();

        [SerializeField]
        private AudioClip dropClip;

        [SerializeField]
        private AudioSource audioSource;

        public string inventorySceneName = "Inventory";

        public KeyCode openInventoryKey = KeyCode.I;

        private void Start()
        {
            PrepareUI();
            PrepareInventoryData();
        }

        private void PrepareInventoryData()
        {
            inventoryData.Initialize();
            inventoryData.OnInventoryUpdated += UpdateInventoryUI;
            foreach (InventoryItem item in initialItems)
            {
                if (item.IsEmpty)
                    continue;
                inventoryData.AddItem(item);
            }
        }

        private void UpdateInventoryUI(Dictionary<int, InventoryItem> inventoryState)
        {
            inventoryUI.ResetAllItems();
            foreach (var item in inventoryState)
            {
                inventoryUI.UpdateData(item.Key, item.Value.item.image, 
                    item.Value.quantity);
            }
        }

        private void PrepareUI()
        {
            inventoryUI.InitializeInventoryUI(inventoryData.Size);
            inventoryUI.OnDescriptionRequested += HandleDescriptionRequest;
            inventoryUI.OnSwapItems += HandleSwapItems;
            inventoryUI.OnStartDragging += HandleDragging;
            inventoryUI.OnItemActionRequested += HandleItemActionRequest;
        }

        private void HandleItemActionRequest(int itemIndex)
        {
            //InventoryItem inventoryItem = inventoryData.GetItemAt(itemIndex);
            //if (inventoryItem.IsEmpty)
            //    return;

            //IItemAction itemAction = inventoryItem.item as IItemAction;
            //if(itemAction != null)
            //{
                
            //    inventoryUI.ShowItemAction(itemIndex);
            //    inventoryUI.AddAction(itemAction.ActionName, () => PerformAction(itemIndex));
            //}

            //IDestroyableItem destroyableItem = inventoryItem.item as IDestroyableItem;
            //if (destroyableItem != null)
            //{
            //    inventoryUI.AddAction("Drop", () => DropItem(itemIndex, inventoryItem.quantity));
            //}

        }

        private void DropItem(int itemIndex, int quantity)
        {
            inventoryData.RemoveItem(itemIndex, quantity);
            inventoryUI.ResetSelection();
            audioSource.PlayOneShot(dropClip);
        }

        public void PerformAction(int itemIndex)
        {
            //InventoryItem inventoryItem = inventoryData.GetItemAt(itemIndex);
            //if (inventoryItem.IsEmpty)
            //    return;

            //IDestroyableItem destroyableItem = inventoryItem.item as IDestroyableItem;
            //if (destroyableItem != null)
            //{
            //    inventoryData.RemoveItem(itemIndex, 1);
            //}

            //IItemAction itemAction = inventoryItem.item as IItemAction;
            //if (itemAction != null)
            //{
            //    itemAction.PerformAction(gameObject, inventoryItem.itemState);
            //    audioSource.PlayOneShot(itemAction.actionSFX);
            //    if (inventoryData.GetItemAt(itemIndex).IsEmpty)
            //        inventoryUI.ResetSelection();
            //}
        }

        private void HandleDragging(int itemIndex)
        {
            InventoryItem inventoryItem = inventoryData.GetItemAt(itemIndex);
            if (inventoryItem.IsEmpty)
                return;
            inventoryUI.CreateDraggedItem(inventoryItem.item.image, inventoryItem.quantity);
        }

        private void HandleSwapItems(int itemIndex_1, int itemIndex_2)
        {
            inventoryData.SwapItems(itemIndex_1, itemIndex_2);
        }

        private void HandleDescriptionRequest(int itemIndex)
        {
            InventoryItem inventoryItem = inventoryData.GetItemAt(itemIndex);
            if (inventoryItem.IsEmpty)
            {
                inventoryUI.ResetSelection();
                return;
            }
            Item item = inventoryItem.item;
            string description = PrepareDescription(inventoryItem);
            inventoryUI.UpdateDescription(itemIndex, item.image,
                item.name, description);
        }

        private string PrepareDescription(InventoryItem inventoryItem)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(inventoryItem.item.itemDescription);
            sb.AppendLine();
            for (int i = 0; i < inventoryItem.itemState.Count; i++)
            {
                sb.Append($"{inventoryItem.itemState[i].itemParameter.ParameterName} " +
                    $": {inventoryItem.itemState[i].value} / " +
                    $"{inventoryItem.item.DefaultParametersList[i].value}");
                sb.AppendLine();
            }
            return sb.ToString();
        }

        public void Update()
        {
            //if (Input.GetKeyDown(openInventoryKey))
            //{
            //    // Check if the inventory scene is already loaded
            //    Scene inventoryScene = SceneManager.GetSceneByName(inventorySceneName);
            //    if (inventoryScene.isLoaded)
            //    {
            //        // Unload the inventory scene
            //        SceneManager.UnloadSceneAsync(inventorySceneName);
            //    }
            //    else
            //    {
            //        // Load the inventory scene additively
            //        SceneManager.LoadScene(inventorySceneName, LoadSceneMode.Additive);
            //        foreach (var item in inventoryData.GetCurrentInventoryState())
            //        {
            //            inventoryUI.UpdateData(item.Key,
            //                item.Value.item.image,
            //                item.Value.quantity);
            //        }
            //    }
            //}
            if (Input.GetKeyDown(openInventoryKey))
            {
                if (inventoryUI.isActiveAndEnabled == false)
                {
                    inventoryUI.Show();
                    foreach (var item in inventoryData.GetCurrentInventoryState())
                    {
                        inventoryUI.UpdateData(item.Key,
                            item.Value.item.image,
                            item.Value.quantity);
                    }
                }
                else
                {
                    inventoryUI.Hide();
                }

            }
        }
    }
}