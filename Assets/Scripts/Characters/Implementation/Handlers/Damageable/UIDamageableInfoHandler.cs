using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Game.Characters.Fields.Stats;
using Game.Characters.UI;
using Game.Core;
using Game.UI;
using UnityEngine;
using Zenject;

namespace Game.Characters.Handlers.Damageable
{
    public class UIDamageableInfoHandler : IHandler<IDamageable>
    {
        [SerializeField] private UIDamageableInfo uiDamageableInfoContract;
        [SerializeField] private float _durationTimerHide;
        
        private HealthField _healthField;
        private UIManager _uiManager;

        private UIDamageableInfo _uiDamageableInfo;
        private CancellationTokenSource _cancellationTokenHide;
        
        [Inject]
        private void Install(UIManager uiManager)
        {
            _uiManager = uiManager;
        }
        
        private void Awake()
        {
            _healthField = _targetData.GetDataField<HealthField>(true);
        }

        private void OnEnable()
        {
            _healthField.OnChanged += OnHealthChanged;
        }

        private void OnDisable()
        {
            _healthField.OnChanged -= OnHealthChanged;
            _cancellationTokenHide?.Cancel();
            HideBar();
        }

        private void OnHealthChanged(float value)
        {
            if (_uiDamageableInfo == null)
            {
                if (value.Equals(_healthField.MaxValue)) return;
                
                _uiDamageableInfo = _uiManager.ShowElement(uiDamageableInfoContract);
                _uiDamageableInfo.Initialize(_targetData);
            }

            float factorHealth = _healthField.Value / _healthField.MaxValue; 
            
            _uiDamageableInfo.SetHealth(factorHealth);
            
            _cancellationTokenHide?.Cancel();
            
            if(_healthField.Value >= _healthField.MaxValue)
            {
                _cancellationTokenHide = new();
                TimerHideBar(_cancellationTokenHide.Token);
            }
        }

        private async void TimerHideBar(CancellationToken token)
        {
            try
            {
                await UniTask.Delay(TimeSpan.FromSeconds(_durationTimerHide), cancellationToken: token);
                
                HideBar();
            }
            catch(OperationCanceledException) {}
        }

        private void HideBar()
        {
            if (_uiDamageableInfo != null)
            {
                _uiManager.HideElement(_uiDamageableInfo);
                _uiDamageableInfo = null;
            }
        }
    }
}