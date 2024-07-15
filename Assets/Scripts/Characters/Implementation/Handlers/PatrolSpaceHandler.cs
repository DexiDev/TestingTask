using Game.Core;
using Game.Data.Fields;
using Game.Extensions;
using Game.Spaces;
using Game.Spaces.Fields;
using UnityEngine;

namespace Game.Characters.Handlers
{
    public class PatrolSpaceHandler : IHandler<UnitController>
    {
        private MovePointField _movePointField;
        private UnitSpaceField _unitSpaceField;

        private void Awake()
        {
            _movePointField =  _targetData.GetDataField<MovePointField>(true);
            _unitSpaceField = _targetData.GetDataField<UnitSpaceField>(true);
        }

        private void OnEnable()
        {
            _movePointField.OnChanged += OnChangedMovePoint;
            _unitSpaceField.OnChanged += UnitSpaceChanged;
            UnitSpaceChanged(_unitSpaceField.Value);
        }

        private void OnDisable()
        {
            _movePointField.OnChanged -= OnChangedMovePoint;
            _unitSpaceField.OnChanged -= UnitSpaceChanged;
            _movePointField.SetValue(Vector3.zero);
        }

        private void OnChangedMovePoint(Vector3 value)
        {
            if (value != Vector3.zero || _unitSpaceField.Value == null) return;

            var point = _unitSpaceField.Value.SpaceCollider.bounds.GetRandomPoint();
            _movePointField.SetValue(point);
        }
        
        private void UnitSpaceChanged(UnitSpaceController unitSpace)
        {
            OnChangedMovePoint(Vector3.zero);
        }
    }
}