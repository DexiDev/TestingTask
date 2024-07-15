using System;
using Game.Characters.Fields.Stats;
using Game.Data.Fields;
using Sirenix.Utilities;
using UnityEngine;

namespace Game.Characters
{
    public abstract class DamageableAgentController : AgentController, IDamageable
    {
        [SerializeField] private Behaviour[] _aliveComponent;
        
        private HealthField _healthField;
        private IsBattleStateField _isBattleStateField;

        public Transform Instance => transform;
        public HealthField HealthField => _healthField;
        public IsBattleStateField IsBattleStateField => _isBattleStateField;

        public event Action<DamageableAgentController> OnKill;
        
        public bool IsAlive => _healthField.Value > _healthField.MinValue; //TODO: need rectoring
        
        protected override void Awake()
        {
            base.Awake();
            _healthField = GetDataField<HealthField>(true);
            _isBattleStateField = GetDataField<IsBattleStateField>(true);
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            _healthField.SetValue(_healthField.BaseValue);
            _aliveComponent?.ForEach(behaviour => behaviour.enabled = IsAlive);
        }
        
        public virtual void Damage(float count)
        {
            if (!IsAlive) return;

            bool isKill = IsCanKill(count);
            
            _healthField.DecreaseValue(count);

            if (isKill)
            {
                SetMotionMove(false, 0f);
                _aliveComponent?.ForEach(behaviour => behaviour.enabled = !isKill);
                OnKill?.Invoke(this);
            }
        }
        
        public bool IsCanKill(float count)
        {
            return _healthField.Value - count <= _healthField.MinValue;
        }

        public virtual void SetBattleState(bool isBattle)
        {
            IsBattleStateField?.SetValue(isBattle);
        }
    }
}