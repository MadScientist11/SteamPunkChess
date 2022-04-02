﻿using System;

namespace SteampunkChess.NetworkService
{
    public interface INetworkService : IService
    {
        event Action<(string playerName, string playerScore)[]> OnReadyToStartGame;
        bool OfflineMode { get; }
        PhotonServiceSO.NetworkPlayer LocalPlayer { get; }
        
        public bool AutomaticallySyncScene { set; }
        public LobbyCallbacksDispatcher LobbyCallbacksDispatcher { get; }
        public RoomCallbacksDispatcher RoomCallbacksDispatcher { get; }
        public PhotonRPCSender PhotonRPCSender { get; }

        void CreateRoom(string roomName, int timeLimitInSeconds, int playerTeam, string password = null);

        void LoadGame();

        void JoinLobby();
        void JoinRoom(string roomName = null);
    }
}