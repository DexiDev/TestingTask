using UnityEngine;

namespace Game.UI.UIElements
{
    //TODO: need merge for UIButtonWorldPoint
    public class UIElementFollowTransform : UIElementFollowPoint
    {
        private Transform _targetTransform;

        protected override void OnDisable()
        {
            base.OnDisable();
            _targetTransform = null;
        }

        public void SetTarget(Transform target)
        {
            _targetTransform = target;
            SetPoint(_targetTransform.position);
        }

        protected override void LateUpdate()
        {
            if (_targetTransform == null) return;

            SetPoint(_targetTransform.position);
        }
    }
}