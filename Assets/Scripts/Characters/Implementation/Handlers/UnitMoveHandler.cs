using Game.Core;
using Game.Data.Fields;
using UnityEngine;

namespace Game.Characters.Handlers
{
    public class UnitMoveHandler : IHandler<UnitController>
    {
        [SerializeField] private float _speed;
        [SerializeField] private float _rotationSpeed;
        [SerializeField] private float _stoppingDistance;
        
        private MovePointField _movePointField;

        private void Awake()
        {
            _movePointField = _targetData.GetDataField<MovePointField>(true);
        }

        private void Update()
        {
            var targetPosition = _movePointField.Value;

            if (targetPosition == Vector3.zero)
            {
                _targetData.SetMotionMove(false);
                return;
            }

            if (Vector3.Distance(_targetData.transform.position, targetPosition) <= _stoppingDistance)
            {
                _movePointField.SetValue(Vector3.zero);
                _targetData.SetMotionMove(false);
                return;
            }
            
            _targetData.Move(_targetData.transform.forward * _speed * Time.deltaTime);

            var targetDirection = targetPosition - _targetData.transform.position;

            targetDirection.y = _targetData.transform.position.y;
            
            var targetRotation = Quaternion.LookRotation(targetDirection);

            var rotation = Quaternion.RotateTowards(_targetData.transform.rotation, targetRotation,_rotationSpeed * Time.deltaTime);
            
            _targetData.SetRotation(rotation);
            
            _targetData.SetMotionMove(true, 1f);
        }
    }
}