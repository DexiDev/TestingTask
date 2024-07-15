using System;
using System.Diagnostics;
using UnityEngine;

namespace Game.Data.Attributes
{
    //example [SerializeField, DataID(typeof(ItemsConfig))] private string _itemID;
    
    [AttributeUsage(AttributeTargets.Field)]
    [Conditional("UNITY_EDITOR")]
    public class DataIDAttribute : PropertyAttribute
    {
        private Type _keyDataType;
        public Type KeyDataType => _keyDataType;
        
        public DataIDAttribute(Type keyDataType)
        {
            _keyDataType = keyDataType;
        }

    }
}