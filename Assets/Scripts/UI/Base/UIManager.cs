using System.Collections.Generic;
using System.Linq;
using Game.Assets;
using Sirenix.Utilities;
using UnityEngine;
using Zenject;

namespace Game.UI
{
    public class UIManager
    {
        private UIContainer _uiContainer;
        private AssetsManager _assetsManager;

        private Dictionary<UIElement, List<UIElement>> _poolUIElements = new();
        
        [Inject]
        private void Construct(AssetsManager assetsManager)
        {
            _assetsManager = assetsManager;
        }
        
        public void RegisterContainer(UIContainer uiContainer)
        {
            Transform oldContainer = _uiContainer?.transform;
            
            _poolUIElements.ForEach(contract => contract.Value.ForEach(uiElement =>
            {
                if (uiElement.transform.parent == oldContainer)
                    uiElement.transform.SetParent(uiContainer.transform, true);
            }));
            _uiContainer = uiContainer;
        }

        public void UnregisterContainer(UIContainer uiContainer)
        {
            if (_uiContainer == uiContainer)
                _uiContainer = null;
        }

        public T ShowElement<T>(T uiContract, Transform parent = null) where T : UIElement
        {
            parent ??= _uiContainer?.transform;
            
            var asset = _assetsManager.GetAsset<T>(uiContract, parent);

            var rectTransform = asset.GetComponent<RectTransform>();
            var targetRectTransform = uiContract.GetComponent<RectTransform>();

            rectTransform.pivot = targetRectTransform.pivot;
            rectTransform.anchorMin = targetRectTransform.anchorMin;
            rectTransform.anchorMax = targetRectTransform.anchorMax;
            rectTransform.offsetMin = targetRectTransform.offsetMin;
            rectTransform.offsetMax = targetRectTransform.offsetMax;
            rectTransform.localPosition = targetRectTransform.localPosition;
            rectTransform.localScale = targetRectTransform.localScale;

            asset.OnReleased += OnReleaseElement;

            _poolUIElements.TryAdd(uiContract, new());
            
            _poolUIElements[uiContract].Add(asset);

            asset.OnShow();

            return asset;
        }

        public void HideElement<T>(T uiElement = null) where T : UIElement
        {
            if (uiElement != null)
            {
                uiElement.OnHide();
            }
            // else
            // {
                // _poolUIElements.FirstOrDefault(contract => contract.Key.GetType() == typeof(T)).Value?
                //     .ForEach(element => element.OnHide());
            // }
        }

        private void OnReleaseElement(IAsset asset)
        {
            asset.OnReleased -= OnReleaseElement;

            var uiElement = asset as UIElement;
            
            foreach (var contract in _poolUIElements)
            {
                if (contract.Value.Contains(uiElement))
                {
                    contract.Value.Remove(uiElement);
                    return;
                }
            }
        }

        public bool ContainsElement<T>(T uiElement = null) where T : UIElement
        {
            return uiElement != null ? _poolUIElements.ContainsKey(uiElement) && _poolUIElements[uiElement].Count > 0 : _poolUIElements.Any(contract => contract.Key is T && contract.Value.Count > 0);
        }

        public T[] GetElements<T>(T uiContract) where T : UIElement
        {
            if (!ContainsElement<T>(uiContract)) return null;

            return _poolUIElements[uiContract].Cast<T>().ToArray();
        }

        public T GetElement<T>(T uiContract) where T : UIElement
        {
            return GetElements<T>(uiContract)?.First();
        }
    }
}