using Zenject;

namespace Game.Rewards.Installers
{
    public class RewardsManagerInstaller : MonoInstaller<RewardsManagerInstaller>
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<RewardsManager>().AsSingle();
        }
    }
}