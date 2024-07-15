using Game.UI.UIElements;
using UnityEngine;

namespace Game.Characters.UI
{
    public class UIDamageableInfo : UIElementFollowTransform
    {
        [SerializeField] private UIProgressBar uiProgressBar;

        public void Initialize(IDamageable damageable)
        {
            SetTarget(damageable.Instance);
        }

        public void SetHealth(float factorHealth)
        {
            uiProgressBar.SetValue(factorHealth);
        }
    }
}