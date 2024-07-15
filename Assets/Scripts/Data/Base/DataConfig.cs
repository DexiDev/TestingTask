using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Game.Data
{
    public abstract class DataConfig : SerializedScriptableObject
    {
        #if UNITY_EDITOR
        public abstract DataScriptable[] SystemData { get; }
        #endif
    }

    public abstract class DataConfig<TDataScriptable> : DataConfig where TDataScriptable : DataScriptable
    {
        [SerializeField] protected TDataScriptable[] _datas;
        public TDataScriptable[] Datas => _datas;

        
#if UNITY_EDITOR
        public override DataScriptable[] SystemData => Datas;


        [Button]
        private void FindAllData()
        {
            var guids = UnityEditor.AssetDatabase.FindAssets("t:" + typeof(TDataScriptable).Name);

            List<TDataScriptable> dataScriptables = new();
            
            foreach (var guid in guids)
            {
                string assetPath = UnityEditor.AssetDatabase.GUIDToAssetPath(guid);
                    
                var data = UnityEditor.AssetDatabase.LoadAssetAtPath<TDataScriptable>(assetPath);
                    
                dataScriptables.Add(data);
            }

            _datas = dataScriptables.ToArray();

        }
#endif
    }
}