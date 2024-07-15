using Zenject;

namespace Game.UI.Installers
{
    public class UIManagerInstaller : MonoInstaller<UIManagerInstaller>
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<UIManager>().AsSingle().NonLazy();
        }
    }
}