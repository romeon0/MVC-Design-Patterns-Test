using System;
using UnityEngine;
using UnityEngine.UI;

namespace UnityGame.MVC
{
    public class UIInventorySlot : UIScreen
    {
        [SerializeField] private GameObject _equippedState;
        [SerializeField] private GameObject _unequippedState;
        [SerializeField] private Text _id;
        [SerializeField] private Button _button;

        public string Id { get; private set; }

        public Action<UIInventorySlot> Clicked;

        private void Start()
        {
            _button.onClick.AddListener(OnClicked);
        }

        public void Init(string id)
        {
            Id = id;
            _id.text = id;
        }

        internal void SetEquipped(bool value)
        {
            _equippedState.SetActive(value);
            _unequippedState.SetActive(!value);
        }

        private void OnClicked()
        {
            Clicked?.Invoke(this);
        }
    }
}

