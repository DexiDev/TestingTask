using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Game.Items;
using Game.UI;
using UnityEngine;
using UnityEngine.UI;
using Zenject;
using Random = UnityEngine.Random;

namespace Game.Inventory.UI
{
    public class UIItemMoveInventory : UIElement
    {
        [SerializeField] private Vector3 _minOffset;
        [SerializeField] private Vector3 _maxOffset;
        [SerializeField] private float _speed;
        [SerializeField] private Image _icon;

        private UIManager _uiManager;
        private ItemsManager _itemsManager;
        private InventoryManager _inventoryManager;
        
        private Camera _camera;
        private RectTransform _rectTransform;
        private CancellationTokenSource _cancellationToken;
        
        [Inject]
        private void Install(InventoryManager inventoryManager, ItemsManager itemsManager, UIManager uiManager)
        {
            _uiManager = uiManager;
            _itemsManager = itemsManager;
            _inventoryManager = inventoryManager;
        }
        
        private void Awake()
        {
            _camera = Camera.main;
            _rectTransform = GetComponent<RectTransform>();
        }

        public void Initialize(Vector3 position, KeyValuePair<string, int> item, UIItemInventory target)
        {
            var itemData = _itemsManager.GetData(item.Key);
            
            _icon.sprite = itemData.Icon;
            
            SetPosition(position);

            _cancellationToken?.Cancel();
            _cancellationToken = new();
            MoveToTarget(target, item, _cancellationToken.Token);
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            _cancellationToken?.Cancel();
        }

        private void SetPosition(Vector3 position)
        {
            var offset = new Vector3
            (
                Random.Range(_minOffset.x, _maxOffset.x),
                Random.Range(_minOffset.y, _maxOffset.y),
                Random.Range(_minOffset.z, _maxOffset.z)
            );
            
            _rectTransform.position = _camera.WorldToScreenPoint(position) + offset;
        }

        private async void MoveToTarget(UIItemInventory uiItemInventory, KeyValuePair<string, int> item, CancellationToken token)
        {
            var target = uiItemInventory.Icon.transform;
            try
            {
                while (transform.position != target.position)
                {
                    transform.position = Vector3.MoveTowards(transform.position, target.position,
                        Time.deltaTime * _speed);
                    await UniTask.Yield();
                    
                }
                _uiManager.HideElement(this);
                uiItemInventory.TriggerChange();
            }
            catch(OperationCanceledException) {}
            
            _inventoryManager.Add(item.Key, item.Value);
        }
    }
}