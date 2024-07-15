using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Game.Core;
using Game.Data.Fields;
using Game.Extensions;
using Game.Gameplay;
using Game.Spaces;
using Game.Spaces.Fields;
using Unity.VisualScripting;
using UnityEngine;

namespace Game.Characters.Handlers.Creatures
{
    public class EscapeSpaceHandler : IHandler<EnemyController>
    {
        [SerializeField] private PatrolSpaceHandler _patrolSpaceHandler;
        [SerializeField] private float _durationTimer = 2.5f;
        private MovePointField _movePointField;
        private UnitSpaceField _unitSpaceField;
        private IsBattleStateField _battleStateField;
        private CancellationTokenSource _cancellationTokenTimer;
        
        private void Awake()
        {
            _movePointField =  _targetData.GetDataField<MovePointField>(true);
            _unitSpaceField = _targetData.GetDataField<UnitSpaceField>(true);
            _battleStateField = _targetData.GetDataField<IsBattleStateField>(true);
        }

        private void OnEnable()
        {
            OnBattleStateFieldChanged(_battleStateField.Value);
            
            _movePointField.OnChanged += OnChangedMovePoint;
            _unitSpaceField.OnChanged += UnitSpaceChanged;
            _battleStateField.OnChanged += OnBattleStateFieldChanged;
        }

        private void OnDisable()
        {
            _movePointField.OnChanged -= OnChangedMovePoint;
            _unitSpaceField.OnChanged -= UnitSpaceChanged;
            _battleStateField.OnChanged -= OnBattleStateFieldChanged;
            _cancellationTokenTimer?.Cancel();

            if (!_patrolSpaceHandler.IsDestroyed()) _patrolSpaceHandler.enabled = true;
        }

        private void UnitSpaceChanged(UnitSpaceController unitSpace)
        {
            OnBattleStateFieldChanged(_battleStateField.Value);
        }

        private void OnBattleStateFieldChanged(bool value)
        {
            _cancellationTokenTimer?.Cancel();
            if (value)
            {
                _patrolSpaceHandler.enabled = false;
                OnChangedMovePoint(_movePointField.Value);
            }
            else TimerPatrolEnabled();
        }
        
        private void OnChangedMovePoint(Vector3 value)
        {
            if (value != Vector3.zero || _unitSpaceField.Value == null ||
                (!_battleStateField.Value && _cancellationTokenTimer?.IsCancellationRequested != false)) return;
            
            var point = _unitSpaceField.Value.SpaceEscapeCollider.bounds.GetRandomPoint();
            _movePointField.SetValue(point);
        }

        private async void TimerPatrolEnabled()
        {
            _cancellationTokenTimer?.Cancel();
            _cancellationTokenTimer = new();
            try
            {
                await UniTask.Delay(TimeSpan.FromSeconds(_durationTimer), cancellationToken: _cancellationTokenTimer.Token);
                _patrolSpaceHandler.enabled = true;
                _cancellationTokenTimer?.Cancel();
            }
            catch(OperationCanceledException) {}
            
        }
    }
}