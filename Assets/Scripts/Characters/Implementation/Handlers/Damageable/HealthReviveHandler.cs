using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Game.Characters.Fields.Stats;
using Game.Core;
using UnityEngine;

namespace Game.Characters.Handlers.Damageable
{
    public class HealthReviveHandler : IHandler<IDamageable>
    {
        [SerializeField] private float _durationRevive = 1f;
        
        private HealthField _healthField;
        private CancellationTokenSource _cancellationTokenTimer;

        private float _currentHP;
        
        private void Awake()
        {
            _healthField = _targetData.GetDataField<HealthField>(true);
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

                if (!_targetData.IsAlive) return;
                
                float startHealth = _healthField.Value;
                float remHealthFactor = 1f - _healthField.Value / _healthField.MaxValue;
                float duration = _durationRevive * remHealthFactor;
                float time = duration;
                while (time > 0)
                {
                    float health = Mathf.Lerp(_healthField.MaxValue, startHealth, time / duration);
                    _healthField.SetValue(health);
                    time -= Time.deltaTime;
                    await UniTask.Yield();
                }
                _healthField.SetValue(_healthField.MaxValue);
            }
            catch (OperationCanceledException) { }
        }
    }
}