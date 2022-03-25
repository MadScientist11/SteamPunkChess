using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

namespace SteampunkChess
{
    public class Lobby : ILobbyCallbacks, Zenject.IInitializable, Zenject.ILateDisposable
    {
        private readonly RoomListingMenu _roomListingMenu;

        public Lobby(RoomListingMenu roomListingMenu)
        {
            _roomListingMenu = roomListingMenu;
        }
        public void Initialize()
        {
            PhotonNetwork.AddCallbackTarget(this);
        }

        public void LateDispose()
        {
            PhotonNetwork.RemoveCallbackTarget(this);
        }

        public void Start()
        {
            
        }
        
     

      
        public void OnJoinedLobby()
        {
            Debug.LogError("Client joined lobby");
        }

        public void OnLeftLobby()
        {
            throw new System.NotImplementedException();
        }

        public void OnRoomListUpdate(List<RoomInfo> roomList)
        {
            _roomListingMenu.UpdateMoveListings(roomList);
        }

        public void OnLobbyStatisticsUpdate(List<TypedLobbyInfo> lobbyStatistics)
        {
            
        }
    }
}