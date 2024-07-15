using Zenject;

namespace Game.Inventory.Installers
{
    public class InventoryManagerInstaller : MonoInstaller<InventoryManagerInstaller>
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<InventoryManager>().AsSingle().NonLazy();
        }
    }
}