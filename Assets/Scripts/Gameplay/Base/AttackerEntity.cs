using System;
using Game.Characters;
using MimicSpace;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Game.Gameplay
{
    [RequireComponent(typeof(Leg))]
    public class AttackerEntity : UnitController
    {
        [SerializeField, TabGroup("Component")] private Leg _mimicLeg;
        
        private PlayerController _playerController;
        private IDamageable _target;

        public Leg MimicLeg => _mimicLeg;
        public IDamageable Target => _target;
        

        public event Action<IDamageable> OnTargetChanged;

        public void Initialize(PlayerController playerController)
        {
            _playerController = playerController;
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            _target = null;
        }

        public void SetTarget(IDamageable target)
        {
            if (target == null)
            {
                _target?.SetBattleState(false);
                Release();
            }
            else
            {
                _target = target;
                
                _target.SetBattleState(true);
                _mimicLeg.footPosition = _target.Instance.position;   
            }
            
            OnTargetChanged?.Invoke(target);
        }

        private void LateUpdate()
        {
            if (_target != null)
            {
                _mimicLeg.footPosition = _target.Instance.position;
            }
        }

        public void Release()
        {
            _mimicLeg.Release();
        }
    }
}