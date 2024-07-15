using UnityEngine;

namespace Game.Data
{
    // [Serializable]
    public abstract class TransformField : DataField<Transform>
    {
        protected override bool Equals(Transform oldValue, Transform newValue)
        {
            return oldValue == newValue;
        }
    }
}