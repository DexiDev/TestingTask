using Game.Data;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Game.Characters.Fields.Stats
{
    public class HealthField : FloatField, IBaseValue<float>, IMinMaxValue<float>
    {
        [SerializeField, ReadOnly] private float _baseValue;
        
        public float MinValue => 0;
        
        public float MaxValue => _baseValue;

        public float BaseValue => _baseValue;

        
        public override void SetValue(float value)
        {
            var newValue = ClampValue(value);
            base.SetValue(newValue);
        }

        public override void SetInstance(IDataField dataField)
        {
            if (dataField is IBaseValue<float> baseValue)
            {
                if (!_baseValue.Equals(baseValue.BaseValue))
                {
                    _baseValue = baseValue.BaseValue;
                }
            }
            
            base.SetInstance(dataField);
        }

        public float ClampValue(float value)
        {
            return Mathf.Clamp(value, MinValue, MaxValue);
        }

        protected override void OnValueChanged(float value)
        {
            _baseValue = value;
            base.OnValueChanged(value);
        }
    }
}