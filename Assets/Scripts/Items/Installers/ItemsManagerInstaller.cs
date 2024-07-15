using UnityEngine;
using Zenject;

namespace Game.Items.Installers
{
    public class ItemsManagerInstaller : MonoInstaller<ItemsManagerInstaller>
    {
        [SerializeField] private ItemsConfig _config;
        
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<ItemsManager>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<ItemsConfig>().FromInstance(_config).AsSingle();
        }
    }
}