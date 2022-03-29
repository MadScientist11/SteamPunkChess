using Photon.Realtime;
using SteampunkChess.PopUpService;
using UnityEngine;
using Zenject;

namespace SteampunkChess
{
    public class CreateOrRandomGameWindow : MonoBehaviour
    {
        private Lobby _lobby;
        private IPopUpService _popUpService;

        [Inject]
        private void Construct(Lobby lobby, IPopUpService popUpService)
        {
            _popUpService = popUpService;
            _lobby = lobby;
        }

        public void CreateRoom()
        {
            _popUpService.ShowPopUp(GameConstants.PopUps.RoomCreationWindow);
        }

        public void JoinRandomGame()
        {
            _lobby.JoinRoom();
        }
    }
}
