using Game.Data.Attributes;
using Game.Items;
using Game.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Game.Inventory.UI
{
    public class UIItemInventory : UIElement
    {
        [SerializeField, DataID(typeof(ItemsConfig))] private string _itemID;
        [SerializeField] private TMP_Text _textField;
        [SerializeField] private Image _icon;
        [SerializeField] private bool _isMainInventory;
        [SerializeField] private Animator _animator;
        [SerializeField] private string _pulseAnimationKey;
        
        private InventoryManager _inventoryManager;
        private ItemsManager _itemsManager;

        public Image Icon => _icon;

        [Inject]
        private void Install(InventoryManager inventoryManager, ItemsManager itemsManager)
        {
            _inventoryManager = inventoryManager;
            _itemsManager = itemsManager;
        }

        private void Start()
        {
            _textField.text = _inventoryManager.GetItemCount(_itemID).ToString();
        }

        private void OnEnable()
        {
            if(_icon != null ) _icon.sprite = _itemsManager.GetData(_itemID).Icon;
            
            OnItemChanged(_itemID, _inventoryManager.GetItemCount(_itemID));
            
            _inventoryManager.OnItemAdded += OnItemChanged;
            _inventoryManager.OnItemRemoved += OnItemChanged;
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            
            _inventoryManager.OnItemAdded -= OnItemChanged;
            _inventoryManager.OnItemRemoved -= OnItemChanged;
        }

        private void OnItemChanged(string item, int count)
        {
            if (item != _itemID) return;

            _textField.text = _inventoryManager.GetItemCount(item).ToString();
        }

        public void TriggerChange()
        {
            _animator?.SetTrigger(_pulseAnimationKey);
        }
    }
}