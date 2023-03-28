using System;
using System.Collections.Generic;
using UnityEngine;

namespace UnityGame.MVC
{
    public class InventoryView : MonoBehaviour, IView
    {
        [SerializeField] private InventoryController _controller;

        public Action<string, bool> ItemUpdated;

        private void Start()
        {
            _controller.ItemEquipped += OnItemEquipped;
            _controller.ItemUnequipped += OnItemUnequipped;
        }

        public void EquipItem(string id)
        {
            _controller.EquipItem(id);
        }

        public void UnequipItem(string id)
        {
            _controller.UnequipItem(id);
        }

        public IList<InventoryItem> GetItems()
        {
            return _controller.GetItems();
        }

        public bool IsEquipped(string id)
        {
            return _controller.IsEquipped(id);
        }


        #region Controller's Events
        private void OnItemEquipped(InventoryItem item)
        {
            ItemUpdated?.Invoke(item.id, item.equipped);
        }

        private void OnItemUnequipped(InventoryItem item)
        {
            ItemUpdated?.Invoke(item.id, item.equipped);
        }
        #endregion
    }
}

