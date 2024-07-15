using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Game.Assets;
using Game.Characters.Fields.Stats;
using Game.Core;
using Game.Gameplay;
using UnityEngine;
using Zenject;

namespace Game.Characters.Handlers.Attacker
{
    public class TargetKillJumpingHandler : IHandler<AttackerEntity>
    {
        [SerializeField] private float _durationJump;
        [SerializeField] private AnimationCurve _jumpCurve;
        [SerializeField] private float _jumpFactor = 5f;

        private AssetsManager _assetsManager;
        
        private IDamageable _damageable;
        private CancellationTokenSource _cancellationToken = new();

        [Inject]
        private void Install(AssetsManager assetsManager)
        {
            _assetsManager = assetsManager;
        }

        private void OnEnable()
        {
            OnAttackerTarget(_targetData.Target);
            _targetData.OnTargetChanged += OnAttackerTarget;
        }

        private void OnDisable()
        {
            _targetData.OnTargetChanged -= OnAttackerTarget;
            OnAttackerTarget(null);
        }
        

        private void OnAttackerTarget(IDamageable damageable)
        {
            if (_damageable != null)
            {
                var healthField = _damageable.GetDataField<HealthField>(true);
                if (healthField != null)
                {
                    healthField.OnChanged -= OnDamageableHealthChange;
                }
                
            }

            _damageable = damageable;
            
            if (_damageable != null)
            {
                var healthField = _damageable.GetDataField<HealthField>(true);
                OnDamageableHealthChange(healthField.Value);
                healthField.OnChanged += OnDamageableHealthChange;
            }
        }

        private void OnDamageableHealthChange(float health)
        {
            if (!_damageable.IsAlive)
            {
                _cancellationToken?.Cancel();
                _cancellationToken = new();
                JumpToPlayer(_damageable, _cancellationToken.Token);
            }
        }

        private async void JumpToPlayer(IDamageable damageable, CancellationToken token)
        {
            try
            {
                Vector3 startPosition = damageable.Instance.position;

                float distance = Vector3.Distance(startPosition, _targetData.transform.parent.position);

                float duration = _durationJump * distance;
                float time = duration;

                while (time > 0)
                {
                    var step = Mathf.SmoothStep(1f, 0f, time / duration);
                    var stepCurve = _jumpCurve.Evaluate(step);
                    
                    damageable.Instance.position = Vector3.Lerp(startPosition, _targetData.transform.parent.position, step);
                    damageable.Instance.position += Vector3.up * _jumpFactor * distance * stepCurve;
                    
                    time -= Time.deltaTime;
                    
                    await UniTask.Yield(token);
                }

                _assetsManager.ReleaseAsset(damageable);
            }
            catch (OperationCanceledException) { }
        }
    }
}