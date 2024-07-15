using System;
using Game.Boot.Scenes;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game.Boot
{
    [Serializable]
    public class LoadingParameters
    {
        [SerializeField] private SceneField _sceneAsset;
        
        [SerializeField] private LoadSceneMode _loadSceneMode = LoadSceneMode.Additive;

        [SerializeField] private bool _isActive;
        
        public SceneField SceneAsset => _sceneAsset;

        public LoadSceneMode LoadSceneMode => _loadSceneMode;

        public bool IsActive => _isActive;
    }
}