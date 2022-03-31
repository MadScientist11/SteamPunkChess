using SteampunkChess.NetworkService;
using SteampunkChess.PopUps;
using SteampunkChess.PopUpService;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

namespace SteampunkChess
{
    public class RoomCreationPopUp : PopUp, IPointerClickHandler
    {
        private readonly System.Random _random = new System.Random();
        
        [Header("RoomCreationPopUp")]
        [SerializeField] private TMP_InputField _roomNameInputField;
        [SerializeField] private TMP_InputField _passwordInputField;
        [SerializeField] private TMP_Dropdown _timeDropdown;
        
        private INetworkService _networkService;
        private IPopUpService _popUpService;
        
        private Team _localPlayerTeam = Team.Random;
        private int _matchTimeLimitInSeconds;
        

        [Inject]
        private void Construct(INetworkService networkService, IPopUpService popUpService)
        {
            _popUpService = popUpService;
            _networkService = networkService;
        }

        public override void Start()
        {
            base.Start();
            ChangeRoomTime();
        }

        public void ChangeLocalPlayerTeam(int team)
        {
            _localPlayerTeam = (Team) team switch
            {
                Team.Random when _random.NextDouble() >= 0.5 => Team.White,
                Team.Random => Team.Black,
                _ => (Team) team
            };
        }

        public void ChangeRoomTime()
        {
            _matchTimeLimitInSeconds = _timeDropdown.value switch
            {
                0 => 60,
                1 => 300,
                2 => 600,
                3 => 0,
                _ => -1
            };
            
        }

        public void CreateRoom()
        {
            RoomData roomData = new RoomData()
            {
                RoomName = _roomNameInputField.text,
                Password = _passwordInputField.text,
                TimeLimitInSeconds = _matchTimeLimitInSeconds,
            };
            _networkService.CreateRoom(roomData.RoomName, roomData.Password, roomData.TimeLimitInSeconds);
            _networkService.LocalPlayer.PlayerTeam = (int) _localPlayerTeam;
            _popUpService.HidePopUp(GameConstants.PopUps.RoomCreationWindow, HideType.HideDestroyAndRelease);
        }
        
        public void OnPointerClick(PointerEventData eventData)
        {
            _popUpService.HidePopUp(GameConstants.PopUps.RoomCreationWindow, HideType.HideDestroyAndRelease);
        }
    }
}
