using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UnityGame.MVC
{
    public class UIInventoryScreen : UIScreen
    {
        [SerializeField] private UIInventorySlot _itemPrefab;
        [SerializeField] private GameObject _itemsContainer;
        [SerializeField] private Button _close;
        private InventoryView _view;
        private GameplayView _gameplayView;
        private List<UIInventorySlot> _slots = new List<UIInventorySlot>();

        private void Start()
        {
            _close.onClick.AddListener(Hide);
        }

        protected override void ShowInternal()
        {
            _view.ItemUpdated += OnItemUpdated;
            foreach (InventoryItem item in _view.GetItems())
            {
                UIInventorySlot slot = Instantiate(_itemPrefab, _itemsContainer.transform);
                slot.Init(item.id);
                slot.SetEquipped(item.equipped);
                slot.Clicked += OnSlotClicked;
                _slots.Add(slot);
            }
            _gameplayView.PauseGame();
        }

        protected override void HideInternal()
        {
            _view.ItemUpdated -= OnItemUpdated;
            foreach(var slot in _slots)
            {
                Destroy(slot.gameObject);
            }
            _slots.Clear();
            _gameplayView.ResumeGame();
        }

        private void OnSlotClicked(UIInventorySlot slot)
        {
            if (_view.IsEquipped(slot.Id))
            {
                _view.UnequipItem(slot.Id);
            }
            else
            {
                _view.EquipItem(slot.Id);
            }
        }

        #region View's Events
        private void OnItemUpdated(string itemId, bool equipped)
        {
            UIInventorySlot slot = _slots.Find(e => e.Id == itemId);
            slot.SetEquipped(equipped);
        }
        #endregion
    }
}

