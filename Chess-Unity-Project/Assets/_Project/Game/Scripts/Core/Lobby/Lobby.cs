using Photon.Pun;
using Photon.Realtime;
using SteampunkChess.NetworkService;
using UnityEngine;

namespace SteampunkChess
{
    public class RoomData
    {
        public string RoomName;
        public string Password;
        public string Time;
    }
    
    [CreateAssetMenu(fileName = "PlayerDataSO", menuName = "ScriptableObjects/PlayerDataSO")]
    public class PlayerDataSO : ScriptableObject
    {
        public int team;
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
            _networkService.JoinLobby();
        }

        public void CreateRoom(RoomData roomData)
        {
            _networkService.CreateRoom(roomData.RoomName, roomData.Password, roomData.Time);
            
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