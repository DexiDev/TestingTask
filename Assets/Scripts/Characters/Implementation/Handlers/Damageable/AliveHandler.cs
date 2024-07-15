using Game.Characters.Fields.Stats;
using Game.Core;
using Sirenix.Utilities;
using UnityEngine;

namespace Game.Characters.Handlers
{
    public class AliveHandler : IHandler<IDamageable>
    {
        [SerializeField] private Behaviour[] _componentsActive;
        [SerializeField] private Collider[] _coliidersActive;
        
        private HealthField _healthField;
        private bool _isAlive;
        
        private void Awake()
        {
            _healthField = _targetData.GetDataField<HealthField>(true);
        }

        private void OnEnable()
        {
            _healthField.OnChanged += OnHealthChanged;
            OnHealthChanged(_healthField.Value);
        }

        private void OnDisable()
        {
            _healthField.OnChanged -= OnHealthChanged;
        }

        private void OnHealthChanged(float value)
        {
            if (_isAlive != _targetData.IsAlive)
            {
                _isAlive = _targetData.IsAlive;
                _componentsActive.ForEach(component => component.enabled = _isAlive);
                _coliidersActive.ForEach(component => component.enabled = _isAlive);
            }
            
        }
    }
}