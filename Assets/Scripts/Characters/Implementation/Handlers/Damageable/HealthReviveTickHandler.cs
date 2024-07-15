using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Game.Characters.Fields.Stats;
using Game.Core;
using UnityEngine;

namespace Game.Characters.Handlers.Damageable
{
    public class HealthReviveTickHandler : IHandler<IDamageable>
    {
        [SerializeField] private float _delayStartRevive = 0f;
        [SerializeField] private int _delayPerTickMS;
        
        private HealthField _healthField;
        private HealthPerTickField _healthPerTick;
        private CancellationTokenSource _cancellationTokenTimer;

        private float _currentHP;
        
        private void Awake()
        {
            _healthField = _targetData.GetDataField<HealthField>(true);
            _healthPerTick = _targetData.GetDataField<HealthPerTickField>(true);
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
                _cancellationTokenTimer?.Cancel();
                _cancellationTokenTimer = new();
                ReviveHealth(_cancellationTokenTimer.Token);
            }
            _currentHP = value;
        }

        private async void ReviveHealth(CancellationToken token)
        {
            try
            {
                await UniTask.WaitWhile(() => _targetData.IsBattleStateField.Value, cancellationToken: token);
                
                if(_delayStartRevive > 0f) await UniTask.Delay(TimeSpan.FromSeconds(_delayStartRevive), cancellationToken: token);
                
                while (_healthField.Value < _healthField.MaxValue && _targetData.IsAlive && !token.IsCancellationRequested)
                {
                    _healthField.IncreaseValue(_healthPerTick.Value);
                    await UniTask.Delay(_delayPerTickMS, cancellationToken: token);
                }
            }
            catch (OperationCanceledException) { }
        }
    }
}