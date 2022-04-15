using System;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

namespace SteampunkChess.NetworkService
{
    public class LobbyCallbacksDispatcher : MonoBehaviour, ILobbyCallbacks
    {
        public event Action<List<RoomInfo>> OnRoomListUpdateEvent;
        
        private void OnEnable()
        {
            PhotonNetwork.AddCallbackTarget(this);
        }

        private void OnDisable()
        {
            PhotonNetwork.RemoveCallbackTarget(this);
            ClearLobbyCallbacksEvents();
        }

        public void ClearLobbyCallbacksEvents()
        {
            OnRoomListUpdateEvent = null;
        }

        #region LobbyCallbacks

        public void OnJoinedLobby()
        {
            Logger.Debug("Joined lobby!");
        }

        public void OnLeftLobby()
        {
            Logger.Debug("Left lobby");
            Destroy(gameObject);
        }

        public void OnRoomListUpdate(List<RoomInfo> roomList)
        {
            OnRoomListUpdateEvent?.Invoke(roomList);
        }

        public void OnLobbyStatisticsUpdate(List<TypedLobbyInfo> lobbyStatistics)
        {
        }

        #endregion
    }
}