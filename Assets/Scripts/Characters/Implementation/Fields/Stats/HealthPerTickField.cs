using Game.Data;
using UnityEngine;

namespace Game.Characters.Fields.Stats
{
    public class HealthPerTickField : IntField, IMinMaxValue<int>
    {
        public int MinValue => 0;
        
        public int MaxValue => _value;
        
        public override void SetValue(int value)
        {
            var newValue = ClampValue(value);
            base.SetValue(newValue);
        }
        
        public int ClampValue(int value)
        {
            return Mathf.Clamp(value, MinValue, MaxValue);
        }
    }
}