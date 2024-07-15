using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Game.Characters.Fields.Stats;
using Game.Core;
using UnityEngine;

namespace Game.Characters.Handlers.Creatures
{
    public class EscapeHandler : IHandler<DamageableAgentController>
    {
        [SerializeField] private float _escapeSpeedMult; 
        [SerializeField] private float _timerDuration = 0.5f;
        
        private float _currentHP;
        private HealthField _healthField;
        private CancellationTokenSource _cancellationTokenTimer;

        private float _targetSpeed;
        
        private void Awake()
        {
            _healthField = _targetData.GetDataField<HealthField>(true);
            
            _targetSpeed = _targetData.NavMeshAgent.speed;
        }
        
        private void OnEnable()
        {
            _currentHP = _healthField.Value;
            _healthField.OnChanged += OnHealthChanged;
        }

        private void OnDisable()
        {
            _healthField.OnChanged -= OnHealthChanged;
            _cancellationTokenTimer?.Cancel();
        }
        
        private void OnHealthChanged(float value)
        {
            if (value < _currentHP)
            {
                _targetData.NavMeshAgent.speed = _targetSpeed * _escapeSpeedMult;
                _cancellationTokenTimer?.Cancel();
                _cancellationTokenTimer = new();
                TimerDefaultSpeed(_cancellationTokenTimer.Token);
            }
            _currentHP = value;
        }

        private async void TimerDefaultSpeed(CancellationToken token)
        {
            try
            {
                await UniTask.Delay(TimeSpan.FromSeconds(_timerDuration), cancellationToken: token);
                _targetData.NavMeshAgent.speed = _targetSpeed;
            }
            catch(OperationCanceledException) {}
        }
    }
}