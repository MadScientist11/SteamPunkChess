namespace SteampunkChess.NetworkService
{
    public interface INetworkService : IService
    {
        bool OfflineMode { get; }
        PhotonServiceSO.NetworkPlayer LocalPlayer { get; }
        public bool AutomaticallySyncScene { set; }
        public LobbyCallbacksDispatcher LobbyCallbacksDispatcher { get; }
        public RoomCallbacksDispatcher RoomCallbacksDispatcher { get; }
        public PhotonRPCSender PhotonRPCSender { get; }

        void CreateRoom(string roomName, string password = null, string matchTime = null);

        void JoinLobby();
        void JoinRoom(string roomName = null);
    }
}