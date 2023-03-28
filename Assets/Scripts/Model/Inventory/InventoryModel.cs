using System;
using System.Collections.Generic;

namespace UnityGame.MVC
{
    [Serializable]
    public class InventoryItem
    {
        public string id;
        public bool equipped;
    }

    [Serializable]
    public class InventoryModel
    {
        public List<InventoryItem> items = new List<InventoryItem>();
    }
}

