using Zenject;

namespace Game.Assets.Installers
{
    public class AssetsManagerInstaller : MonoInstaller<AssetsManagerInstaller>
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<AssetsManager>().AsSingle().NonLazy();
        }
    }
}