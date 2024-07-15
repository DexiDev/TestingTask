namespace Game.Data
{
    // [Serializable]
    public class FloatField : DataField<float>
    {
        public void IncreaseValue(float value)
        {
            SetValue(_value + value);
        }
        
        public void DecreaseValue(float value)
        {
            SetValue(_value - value);
        }
    }
}