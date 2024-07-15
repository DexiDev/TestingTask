using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using Zenject;
using IInitializable = Zenject.IInitializable;

namespace Game.Assets
{
    public class AssetsManager : IInitializable
    {
        private Dictionary<IAsset, List<IAsset>> _poolAsset = new();
        private Dictionary<IAsset, IAsset> _activeAsset = new();

        private Transform _container;
        
        private DiContainer _diContainer;
        
        [Inject]
        private void Install(DiContainer diContainer)
        {
            _diContainer = diContainer;
        }
        

        public void Initialize()
        {
            var container = new GameObject("Asset Container")
            {
                active = false
            };
            
            Object.DontDestroyOnLoad(container);
            
            _container = container.transform;
        }
        
        public T GetAsset<T>(T prefab, Transform parent) where T : Object, IAsset
        {
            if(parent != null) return GetAsset<T>(prefab, parent.position, parent.rotation, parent);
            else return GetAsset<T>(prefab, Vector3.zero, Quaternion.identity, parent);
        }

        public T GetAsset<T>(T prefab, Vector3 position, Quaternion rotation, Transform parent) where T : Object, IAsset
        {
            T asset = null;
            
            if(_poolAsset.TryGetValue(prefab, out var poolList))
            {
                asset = poolList.FirstOrDefault() as T;
            }

            if (asset != null)
            {
                _poolAsset[prefab].Remove(asset);
                asset.Asset.transform.position = position;
                asset.Asset.transform.rotation = rotation;
                asset.Asset.transform.SetParent(parent, true);
                asset.Asset.gameObject.SetActive(true);
            }
            else
            {
                var gameObject = _diContainer.InstantiatePrefab(prefab, position, rotation, _container);

                gameObject.transform.SetParent(parent, true);
                
                asset = gameObject.GetComponent<T>();
            }

            asset.OnReleased += OnReleaseAsset;
            
            _activeAsset.Add(asset, prefab);
            
            return asset;
        }

        private void OnReleaseAsset(IAsset asset)
        {
            if (!_activeAsset.ContainsKey(asset)) return;
            
            asset.OnReleased -= OnReleaseAsset;

            var assetContract = _activeAsset[asset];
            
            _activeAsset.Remove(asset);
            
            if (!asset.Asset.IsDestroyed())
            {
                _poolAsset.TryAdd(assetContract, new List<IAsset>());

                _poolAsset[assetContract].Add(asset);

                if (asset.Asset.activeSelf)
                {
                    asset.Asset.SetActive(false);
                }
            }
        }

        public void ReleaseAsset(IAsset asset)
        {
            OnReleaseAsset(asset);
        }
    }
}