using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UnityGame.MVC
{
    [RequireComponent(typeof(Button))]
    public class Player : MonoBehaviour
    {
        public Action Clicked;

        private void Start()
        {
            Button button = GetComponent<Button>();
            button.onClick.AddListener(OnClicked);
        }

        private void OnClicked()
        {
            Clicked?.Invoke();
        }
    }
}

