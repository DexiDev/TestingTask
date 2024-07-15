using Zenject;

namespace Game.Input.Installers
{
    public class InputManagerInstaller : MonoInstaller<InputManagerInstaller>
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<InputManager>().AsSingle().NonLazy();
        }
    }
}