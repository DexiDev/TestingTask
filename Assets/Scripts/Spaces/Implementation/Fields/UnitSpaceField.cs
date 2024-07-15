using Game.Data;

namespace Game.Spaces.Fields
{
    public class UnitSpaceField : DataField<UnitSpaceController>
    {
        protected override bool Equals(UnitSpaceController oldValue, UnitSpaceController newValue)
        {
            return oldValue == newValue;
        }
    }
}