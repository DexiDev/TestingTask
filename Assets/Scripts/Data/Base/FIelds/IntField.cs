namespace Game.Data
{
    // [Serializable]
    public abstract class IntField : DataField<int>
    {
        public void IncreaseValue(int value)
        {
            SetValue(_value + value);
        }
        
        public void DecreaseValue(int value)
        {
            SetValue(_value - value);
        }
    }
}