using Game.Data;
using Game.Data.Attributes;
using Game.Rewards;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Game.Items.Fields
{
    public class ItemField : StringField, IRewardField
    {
        [DataID(typeof(ItemsConfig))]
        [SerializeField, OnValueChanged("OnValueChanged")] protected new string _value;
        
        public override string Value => _value;
    }
}