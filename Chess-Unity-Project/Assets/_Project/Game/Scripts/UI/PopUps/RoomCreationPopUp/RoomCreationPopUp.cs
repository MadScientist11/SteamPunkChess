using SteampunkChess.NetworkService;
using SteampunkChess.PopUps;
using SteampunkChess.PopUpService;
using TMPro;
using UnityEngine;
using Zenject;

namespace SteampunkChess
{
    public class RoomCreationPopUp : PopUp
    {
        [SerializeField] private TextMeshProUGUI _roomNameText;
        [SerializeField] private TextMeshProUGUI _passwordText;
        
        private readonly System.Random _random = new System.Random();
        private Lobby _lobby;
        private IPopUpService _popUpService;

        [Inject]
        private void Construct(Lobby lobby, IPopUpService popUpService)
        {
            _popUpService = popUpService;
            _lobby = lobby;
        }

        public Team GetLocalPlayerTeam(int team)
        {
            return (Team) team switch
            {
                Team.Random when _random.NextDouble() >= 0.5 => Team.White,
                Team.Random => Team.Black,
                _ => (Team) team
            };
        }

        public void CreateRoom()
        {
            _popUpService.HidePopUp(GameConstants.PopUps.RoomCreationWindow, HideType.HideDestroyAndRelease);
            RoomData roomData = new RoomData()
            {
                RoomName = _roomNameText.text,
                Password = _passwordText.text,
                Time = "5:00",
            };
            _lobby.CreateRoom(roomData);
        }

        
    }
}
