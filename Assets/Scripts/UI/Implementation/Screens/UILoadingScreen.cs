using UI.Base;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Implementation.Screens
{
    public class UILoadingScreen : UIScreen
    {
        [SerializeField] private Image _progressBar;

        public void SetProgress(float progress)
        {
            _progressBar.fillAmount = progress;
        }
    }
}