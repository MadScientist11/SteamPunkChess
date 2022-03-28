﻿using System;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

namespace SteampunkChess.NetworkService
{
    public class RoomCallbacksDispatcher : MonoBehaviour, IInRoomCallbacks, IMatchmakingCallbacks
    {
        public event Action<Player> OnPlayerEnteredRoomEvent;
        public event Action OnCreatedRoomEvent;
        public event Action OnJoinedRoomEvent;

        private void OnEnable()
        {
            PhotonNetwork.AddCallbackTarget(this);
        }

        private void OnDisable()
        {
            PhotonNetwork.RemoveCallbackTarget(this);
            OnPlayerEnteredRoomEvent = null;
            OnCreatedRoomEvent = null;
            OnJoinedRoomEvent = null;
           
        }

        #region InRoomCallbacks

        public void OnPlayerEnteredRoom(Player newPlayer)
        {
            OnPlayerEnteredRoomEvent?.Invoke(newPlayer);
            Logger.DebugError("Player entered room!");
        }

        public void OnPlayerLeftRoom(Player otherPlayer)
        {
            
        }

        public void OnRoomPropertiesUpdate(Hashtable propertiesThatChanged)
        {
        }

        public void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
        {
        }

        public void OnMasterClientSwitched(Player newMasterClient)
        {
        }

        #endregion

        #region MatchmakingCallbacks

        public void OnFriendListUpdate(List<FriendInfo> friendList)
        {
        }

        public void OnCreatedRoom()
        {
            OnCreatedRoomEvent?.Invoke();
            Logger.Debug("Room created!");
        }

        public void OnCreateRoomFailed(short returnCode, string message)
        {
        }

        public void OnJoinedRoom()
        {
            OnJoinedRoomEvent?.Invoke();
            Logger.Debug("Joined room!");
        }

        public void OnJoinRoomFailed(short returnCode, string message)
        {
        }

        public void OnJoinRandomFailed(short returnCode, string message)
        {
        }

        public void OnLeftRoom()
        {
            gameObject.SetActive(false);
            Logger.Debug("Room left, destroying room callbacks dispatcher");
        }

        #endregion
    }
}