using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Game.Characters.Fields.Stats;
using Game.Core;
using Game.Gameplay;
using UnityEngine;

namespace Game.Characters.Handlers.Attacker
{
    public class AttackerDamageHandler : IHandler<AttackerEntity>
    {
        [SerializeField] private int _baseDamageTickRate = 510;
        
        private DamagePowerField _damagePowerField;
        private DamageTickRateField _damageTickRateField;
        
        private CancellationTokenSource _cancellationTokenDamage;
        
        private void Awake()
        {
            _damagePowerField = _targetData.GetDataField<DamagePowerField>();
            _damageTickRateField = _targetData.GetDataField<DamageTickRateField>();
        }
        
        private void OnEnable()
        {
            OnAttackerTarget(_targetData.Target);
            _targetData.OnTargetChanged += OnAttackerTarget;
        }

        private void OnDisable()
        {
            _targetData.OnTargetChanged -= OnAttackerTarget;
            _cancellationTokenDamage?.Cancel();
        }

        private void OnAttackerTarget(IDamageable damageable)
        {
            _cancellationTokenDamage?.Cancel();

            if (damageable != null)
            {
                _cancellationTokenDamage = new CancellationTokenSource();
                DamageTick(damageable, _cancellationTokenDamage.Token);
            }
        }
        
        private async void DamageTick(IDamageable damageable, CancellationToken token)
        {
            try
            {
                while (damageable.IsAlive && !token.IsCancellationRequested)
                {
                    damageable.Damage(_damagePowerField.Value);
        
                    if(!damageable.IsAlive) break;
                    
                    float delayMs = _baseDamageTickRate / _damageTickRateField.Value;
                    
                    await UniTask.Delay((int)delayMs, cancellationToken: token);
                }
        
            }
            catch (OperationCanceledException)
            {
                
            }
        }
    }
}