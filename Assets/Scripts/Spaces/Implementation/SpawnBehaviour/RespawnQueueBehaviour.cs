using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Game.Characters;
using UnityEngine;

namespace Game.Spaces.SpawnBehaviour
{
    public class RespawnQueueBehaviour : ISpawnBehaviour
    {
         private const string _saveTimeKill = "TimeKill";
        
        [SerializeField] private float _timerSpawn = 3f;
        
        private UnitSpaceController _unitSpaceController;
        
        private CancellationTokenSource _cancellationTokenTimer;
        
        public void Initialize(UnitSpaceController unitSpaceController)
        {
            _unitSpaceController = unitSpaceController;
        }

        public void OnEnable()
        {
            _unitSpaceController.OnDamageableUnitKill += OnDamageableAgentKill; 
            var timeKill = DateTime.Now;
            
            DelaySpawn(timeKill);
        }

        public void OnDisable()
        {
            _unitSpaceController.OnDamageableUnitKill -= OnDamageableAgentKill; 
            _cancellationTokenTimer?.Cancel();
        }

        private async void DelaySpawn(DateTime dateTime)
        {
            if (_cancellationTokenTimer is { IsCancellationRequested: false } || _unitSpaceController.GetCountKills() <= 0) return;
            
            _cancellationTokenTimer?.Cancel();
            _cancellationTokenTimer = new();

            var targetTime = dateTime + TimeSpan.FromSeconds(_timerSpawn);
            var delayTime = targetTime - DateTime.Now;
            
            try
            {
                if (delayTime.TotalMilliseconds > 0) 
                    await UniTask.Delay(delayTime, cancellationToken: _cancellationTokenTimer.Token);

                _cancellationTokenTimer?.Cancel();
                
                _unitSpaceController.Spawn();
                
                DelaySpawn(targetTime);
            }
            catch(OperationCanceledException) {}
        }

        private void OnDamageableAgentKill(UnitController unitController)
        {
            var timeKill = DateTime.Now;
            
            DelaySpawn(timeKill);
        }
    }
}