using System;
using Game.Assets;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Game.UI
{
    public class UIElement : SerializedMonoBehaviour, IAsset
    {
        public GameObject Asset => gameObject;
        
        public event Action<IAsset> OnReleased;

        protected virtual void OnDisable()
        {
            OnReleased?.Invoke(this);
        }
        
        public virtual void OnShow(){}

        public virtual void OnHide()
        {
            gameObject.SetActive(false);
        }
    }
}