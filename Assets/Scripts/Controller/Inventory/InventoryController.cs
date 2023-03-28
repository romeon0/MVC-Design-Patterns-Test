using System;
using System.Collections.Generic;
using UnityEngine;

namespace UnityGame.MVC
{
    public class InventoryController : MonoBehaviour, IController
    {
        [SerializeField] private InventoryModel _inventory;
        public Action<InventoryItem> ItemEquipped;
        public Action<InventoryItem> ItemUnequipped;

        public void EquipItem(string id)
        {
            InventoryItem item = _inventory.items.Find(e => e.id == id);
            item.equipped = true;
            ItemEquipped?.Invoke(item);
        }

        public void UnequipItem(string id)
        {
            InventoryItem item = _inventory.items.Find(e => e.id == id);
            item.equipped = false;
            ItemUnequipped?.Invoke(item);
        }

        public IList<InventoryItem> GetItems()
        {
            return _inventory.items;
        }

        public bool IsEquipped(string id)
        {
            InventoryItem item = _inventory.items.Find(e => e.id == id);
            return item.equipped;
        }
    }
}

