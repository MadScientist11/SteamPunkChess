using SteampunkChess.PopUps;
using SteampunkChess.PopUpService;
using UnityEngine.AddressableAssets;

namespace SteampunkChess
{
    public class ExitGamePopUp : PopUp
    {
        private IPopUpService _popUpService;
        public override string PopUpKey { get; set; } = GameConstants.PopUps.ExitGamePopUp;

        private void Construct(IPopUpService popUpService)
        {
            _popUpService = popUpService;
        }

        public void StayInGame()
        {
            _popUpService.HidePopUp(GameConstants.PopUps.ExitGamePopUp, HideType.HideAndDestroy);
        }

        public void ExitGame()
        {
            Addressables.LoadSceneAsync("Lobby");
        }
    }
}
