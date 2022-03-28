using SteampunkChess.CloudService;
using SteampunkChess.NetworkService;
using SteampunkChess.PopUpService;
using UnityEngine;
using Zenject;

namespace SteampunkChess
{
    public class BootstrapInstaller : MonoInstaller, Zenject.IInitializable
    {
        [SerializeField] private PopUpServiceSO _popUpService;
        [SerializeField] private PlayFabServiceSO _playFabService;
        [SerializeField] private PhotonServiceSO _photonService;
       
        public override void InstallBindings()
        {
            BindPopUpService();
            BindCloudService();
            BindNetworkService();
            BindInstallerInterfaces();
            BindServiceContainer();
        
        }
        public void Initialize()
        {
            InjectServices();
        }

        private void InjectServices()
        {
            Container
                .Inject(_photonService);
            Container
                .Inject(_playFabService);
            Container
                .Inject(_popUpService);
        }

        private void BindServiceContainer()
        {
            Container
                .Bind<ServiceContainer>()
                .AsSingle();
        }

        private void BindInstallerInterfaces()
        {
            Container
                .BindInterfacesTo<BootstrapInstaller>()
                .FromInstance(this)
                .AsSingle();
        }

        private void BindNetworkService()
        {
            Container
                .BindInterfacesAndSelfTo<PhotonServiceSO>()
                .FromInstance(_photonService)
                .AsSingle();
        }

        private void BindCloudService()
        {
            Container
                .Bind<ICloudService>()
                .To<PlayFabServiceSO>()
                .FromInstance(_playFabService)
                .AsSingle();
        }

        private void BindPopUpService()
        {
            Container
                .Bind<IPopUpService>()
                .To<PopUpServiceSO>()
                .FromInstance(_popUpService)
                .AsSingle();
        }

      
    }
}