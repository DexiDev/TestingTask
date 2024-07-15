using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game.Boot
{
    public class BootLoader : MonoBehaviour
    {
        [SerializeField] private List<LoadingParameters> _loadingParameters;
        [SerializeField] private int _fpsLimit = 999;
        // [SerializeField] private UILoadingScreen _uiLoadingScreen;
        [SerializeField] private float _minDurationLoading = 3f;
        
        private void Awake()
        {
            Application.targetFrameRate = _fpsLimit;
        }

        private async void Start()
        {
            var startTime = DateTime.Now;
            // _uiLoadingScreen.SetProgress(0f);
            
            for (int i = 0; i < _loadingParameters.Count; i++)
            {
                var loadingParameters = _loadingParameters[i];
                
                var loadingScene = SceneManager.LoadSceneAsync(loadingParameters.SceneAsset.SceneName, loadingParameters.LoadSceneMode);

                while (!loadingScene.isDone)
                {
                    float progress = ((float)i / _loadingParameters.Count) + (loadingScene.progress / _loadingParameters.Count);
                    // _uiLoadingScreen.SetProgress(progress);
                    await UniTask.Yield();
                }
                
                if (loadingParameters.IsActive)
                {
                    var scene = SceneManager.GetSceneByName(loadingParameters.SceneAsset.SceneName);
                    SceneManager.SetActiveScene(scene);
                }
            }
            
            var remainderTime = DateTime.Now - startTime;
            if (remainderTime.TotalSeconds < _minDurationLoading)
            {
                var durationTime = _minDurationLoading - remainderTime.TotalSeconds;
                await UniTask.Delay(TimeSpan.FromSeconds(durationTime));
            }
            
            SceneManager.UnloadSceneAsync(gameObject.scene);
        }
    }
}