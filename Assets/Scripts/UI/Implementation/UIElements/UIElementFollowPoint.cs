using UnityEngine;

namespace Game.UI.UIElements
{
    public class UIElementFollowPoint : UIElement
    {
        [SerializeField] private Vector3 _offset;

        private Camera _camera;
        private Vector3 _targetPoint;
        private RectTransform _rectTransform;

        private void Awake()
        {
            _camera = Camera.main;
            _rectTransform = GetComponent<RectTransform>();
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            _targetPoint = Vector3.zero;
        }

        public void SetPoint(Vector3 vector3)
        {
            _targetPoint = vector3;
            UpdateScreenPosition();
        }

        protected virtual void LateUpdate()
        {
            UpdateScreenPosition();
        }

        private void UpdateScreenPosition()
        {
            _rectTransform.position = _camera.WorldToScreenPoint(_targetPoint) + _offset;
        }
    }
}