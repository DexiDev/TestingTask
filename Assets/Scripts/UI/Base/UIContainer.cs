using UnityEngine;
using Zenject;

namespace Game.UI
{
    public class UIContainer : MonoBehaviour
    {
        private UIManager _uiManager;

        [Inject]
        private void Construct(UIManager uiManager)
        {
            _uiManager = uiManager;
        }

        private void Awake()
        {
            _uiManager.RegisterContainer(this);
        }

        private void OnEnable()
        {
            _uiManager.RegisterContainer(this);
        }

        private void OnDisable()
        {
            _uiManager.UnregisterContainer(this);
        }
    }
}