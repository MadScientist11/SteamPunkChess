using SteampunkChess.CloudService;
using SteampunkChess.PopUpService;
using UnityEngine;
using Zenject;

namespace SteampunkChess
{
    public class BootstrapInstaller : MonoInstaller
    {
        [SerializeField] private PopUpServiceSO _popUpService;
        [SerializeField] private PlayFabServiceSO _playFabService;

        public override void InstallBindings()
        {
            Container
                .Bind<IPopUpService>()
                .To<PopUpServiceSO>()
                .AsSingle();

            Container
                .Bind<ICloudService>()
                .To<PlayFabServiceSO>()
                .AsSingle();



        }

    }
}