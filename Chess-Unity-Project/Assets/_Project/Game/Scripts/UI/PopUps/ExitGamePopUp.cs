using SteampunkChess.NetworkService;
using SteampunkChess.PopUps;
using SteampunkChess.PopUpService;
using UnityEngine.AddressableAssets;
using Zenject;

namespace SteampunkChess
{
    public class ExitGamePopUp : PopUp
    {
        private IPopUpService _popUpService;
        private INetworkService _networkService;
        public override string PopUpKey { get; set; } = GameConstants.PopUps.ExitGamePopUp;

        [Inject]
        private void Construct(IPopUpService popUpService, INetworkService networkService)
        {
            _networkService = networkService;
            _popUpService = popUpService;
        }

        public void StayInGame()
        {
            _popUpService.HidePopUp(GameConstants.PopUps.ExitGamePopUp, HideType.HideAndDestroy);
        }

        public void ExitGame()
        {
            _networkService.LeaveRoom();
        }
    }
}
