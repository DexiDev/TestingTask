using System;
using System.Collections.Generic;
using Game.Items;
using Sirenix.Utilities;
using Zenject;

namespace Game.Inventory
{
    public class InventoryManager
    {
        private ItemsManager _itemsManager;
        private Dictionary<string, int> _items = new();

        public event Action<string, int> OnItemChanged;
        public event Action<string, int> OnItemAdded;
        public event Action<string, int> OnItemRemoved;
        
        [Inject]
        private void Install(ItemsManager itemsManager)
        {
            _itemsManager = itemsManager;
        }
        
        public void Add(string targetItem, int count)
        {
            if (count == 0) return;
            
            var itemData = _itemsManager.GetData(targetItem);

            if (itemData != null)
            {
                _items.TryAdd(targetItem, 0);

                var resultCount = _items[targetItem] + count;

                if (itemData.StackLimit != -1) resultCount = Math.Clamp(resultCount, resultCount, itemData.StackLimit);

                var oldCount = _items[targetItem]; 
                
                _items[targetItem] = resultCount;
                
                OnItemChanged?.Invoke(targetItem, resultCount);
                OnItemAdded?.Invoke(targetItem, resultCount - oldCount);
            }
        }

        public bool ContainsItem(string targetItem, int count = 1)
        {
            return GetItemCount(targetItem) >= count;
        }

        public int GetItemCount(string targetItem)
        {
            return _items.GetValueOrDefault(targetItem, 0);
        }

        public int GetItemAllCount()
        {
            int count = 0;

            _items?.ForEach(item => count += item.Value);

            return count;
        }

        public Dictionary<string, int> GetItems()
        {
            return _items;
        }
    }
}