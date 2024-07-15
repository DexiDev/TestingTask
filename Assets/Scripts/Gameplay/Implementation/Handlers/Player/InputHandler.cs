using Game.Core;
using Game.Gameplay;
using Game.Input;
using UnityEngine;
using Zenject;

namespace Game.Characters.Handlers.Player
{
    public class InputHandler : IHandler<PlayerController>
    {
        [SerializeField] private float _rotationSpeed = 8f;
        [SerializeField] private float _stopAcceleration = 5f;

        private float _speed;
        private Vector3 _targetDirection;

        private InputManager _inputManager;

        [Inject]
        private void Install(InputManager inputManager)
        {
            _inputManager = inputManager;
        }

        public void Update()
        {
            if (_inputManager.Direction != Vector2.zero)
            {
                _targetDirection = new Vector3(_inputManager.Direction.x, 0f, _inputManager.Direction.y);
            }

            _speed = _targetDirection.magnitude;

            bool isMoving = _targetDirection != Vector3.zero;
            
            if (isMoving)
            {
                Quaternion targetRotation = Quaternion.LookRotation(_targetDirection, Vector3.up);
                Quaternion rotation = Quaternion.Lerp(_targetData.Transform.rotation, targetRotation,
                    Time.deltaTime * _rotationSpeed);

                _targetData.SetRotation(rotation);

                _targetData.Move(transform.forward * _speed * Time.deltaTime);
            }

            _targetDirection = Vector3.Lerp(_targetDirection, Vector3.zero, Time.deltaTime * _stopAcceleration);

            _targetData.SetMotionMove(isMoving, _speed);
        }
    }
}