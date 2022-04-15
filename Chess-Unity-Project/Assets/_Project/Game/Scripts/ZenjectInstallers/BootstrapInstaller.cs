using SteampunkChess.CloudService;
using SteampunkChess.LocalizationSystem;
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
        [SerializeField] private LocalizationSystemSO _localizationSystem;
        [SerializeField] private InputSystem _inputSystem;
        [SerializeField] private AudioSystem _audioSystem;
       
        public override void InstallBindings()
        {
            BindPopUpService();
            BindCloudService();
            BindNetworkService();
            BindLocalizationService();
            BindInputSystem();
            BindAudioSystem();
            
            BindInstallerInterfaces();
            BindServiceContainer();
            
            Container
                .Bind<PlayFabPlayerData>()
                .AsSingle();
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
            Container
                .Inject(_localizationSystem);
            Container
                .Inject(_inputSystem);
            Container
                .Inject(_audioSystem);
        }
        
        private void BindAudioSystem()
        {
            Container
                .Bind<IAudioSystem>()
                .To<AudioSystem>()
                .FromInstance(_audioSystem)
                .AsSingle();
        }
        private void BindInputSystem()
        {
            Container
                .Bind<IInputSystem>()
                .To<InputSystem>()
                .FromInstance(_inputSystem)
                .AsSingle();
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
        
        private void BindLocalizationService()
        {
            Container
                .Bind<ILocalizationSystem>()
                .To<LocalizationSystemSO>()
                .FromInstance(_localizationSystem)
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