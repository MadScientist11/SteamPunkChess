using SteampunkChess.NetworkService;

namespace SteampunkChess
{
    public class RoomData
    {
        public string RoomName;
        public string Password;
        public int TimeLimitInSeconds;
    }
    
    public class Lobby : IInitializable
    {
        private readonly RoomListingMenu _roomListingMenu;
        private INetworkService _networkService;

        public Lobby(INetworkService networkService, RoomListingMenu roomListingMenu)
        {
            _networkService = networkService;
            _roomListingMenu = roomListingMenu;
        }
        
        public void Initialize()
        {
            _networkService.AutomaticallySyncScene = true;
            _networkService.LobbyCallbacksDispatcher.OnRoomListUpdateEvent += _roomListingMenu.UpdateRoomListing;
            //_networkService.LobbyCallbacksDispatcher.OnCreateRoom
            _networkService.JoinLobby();
        }

        public void OnCreateRoom()
        {
            //loading and waiting for opponent
        }

        public void JoinRoom()
        {
            _networkService.JoinRoom();
        }

        public void SetPlayerData()
        {
            
        }

        public void SetRoomData()
        {
            
        }

      
    }
}