using Cinemachine;
using Game.Core;
using Game.Gameplay;
using Game.Input;
using UnityEngine;
using Zenject;

namespace Game.Characters.Handlers.Player
{
    public class CameraHandler : IHandler<PlayerController>
    {
        [SerializeField] private CinemachineVirtualCamera _cameraMove;

        private InputManager _inputManager;

        [Inject]
        private void Install(InputManager inputManager)
        {
            _inputManager = inputManager;
        }

        private void OnEnable()
        {
            OnDirectionChanged(_inputManager.Direction);
            _inputManager.OnDirectionChanged += OnDirectionChanged;
        }

        private void OnDisable()
        {
            _inputManager.OnDirectionChanged -= OnDirectionChanged;
        }

        private void OnDirectionChanged(Vector2 direction)
        {
            _cameraMove.gameObject.SetActive(direction != Vector2.zero);
        }
    }
}