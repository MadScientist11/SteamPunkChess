using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using Zenject;
using Object = UnityEngine.Object;

namespace SteampunkChess.NetworkService
{
    public interface INetworkService : IService
    {
        bool OfflineMode { get; }

        string Username { get; set; }

        public bool AutomaticallySyncScene { set; }
        public LobbyCallbacksDispatcher LobbyCallbacksDispatcher { get; }
        public RoomCallbacksDispatcher RoomCallbacksDispatcher { get; }
        public PhotonRPCSender PhotonRPCSender { get; }

        void CreateRoom(string roomName, string password = null, string matchTime = null);

        void JoinLobby();
        void JoinRoom(string roomName = null);
    }

    public interface IRPCSender
    {
        void SendRPC(byte rpcCode, object[] content, ReceiverGroup receivers, SendOptions sendOptions);
    }

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


    public class RoomCallbacksDispatcher : MonoBehaviour, IInRoomCallbacks, IMatchmakingCallbacks
    {
        public event Action<Player> OnPlayerEnteredRoomEvent;
        public event Action OnCreatedRoomEvent;
        public event Action OnJoinedRoomEvent;
        public event Action OnLeftRoomEvent;

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


    [CreateAssetMenu(fileName = "PhotonServiceSO", menuName = "Services/PhotonServiceSO")]
    public class PhotonServiceSO : ScriptableObject, INetworkService, IConnectionCallbacks
    {
        public string InitializationMessage => "Connecting to the server...";
        
        private LobbyCallbacksDispatcher _lobbyCallbacksDispatcher;
        private RoomCallbacksDispatcher _roomCallbacksDispatcher;
        private PhotonRPCSender _photonRPCSender;
        [SerializeField] private GameObject _photonRPCSenderPrefab;
        [SerializeField] private GameObject _photonPrefabPool;

        public PhotonRPCSender PhotonRPCSender
        {
            get
            {
                if (_photonRPCSender == null)
                {
                    _photonRPCSender = new GameObject("PhotonRPCSender")
                        .AddComponent<PhotonRPCSender>();
                }

                return _photonRPCSender;
            }
           
        }

        public LobbyCallbacksDispatcher LobbyCallbacksDispatcher
        {
            get
            {
                if (_lobbyCallbacksDispatcher == null)
                {
                    _lobbyCallbacksDispatcher = new GameObject("LobbyCallbacksDispatcher")
                        .AddComponent<LobbyCallbacksDispatcher>();
                    DontDestroyOnLoad(_lobbyCallbacksDispatcher);
                }

                return _lobbyCallbacksDispatcher;
            }
        }

        public RoomCallbacksDispatcher RoomCallbacksDispatcher
        {
            get
            {
                if (_roomCallbacksDispatcher == null)
                {
                    _roomCallbacksDispatcher = new GameObject("RoomCallbacksDispatcher")
                        .AddComponent<RoomCallbacksDispatcher>();
                    DontDestroyOnLoad(_roomCallbacksDispatcher);
                }
                
                if(!_roomCallbacksDispatcher.gameObject.activeSelf)
                    _roomCallbacksDispatcher.gameObject.SetActive(true);

                return _roomCallbacksDispatcher;
            }
        }

        public bool OfflineMode => PhotonNetwork.OfflineMode;

        public bool AutomaticallySyncScene
        {
            set => PhotonNetwork.AutomaticallySyncScene = value;
        }

        public string Username
        {
            get => PhotonNetwork.NickName;
            set => PhotonNetwork.NickName = value;
        }

        [Inject]
        private void Construct(ServiceContainer serviceContainer)
        {
            serviceContainer.ServiceList.Add(this);
        }

        public async Task Initialize()
        {
            PhotonNetwork.AddCallbackTarget(this);
            
            PhotonNetwork.ConnectUsingSettings();

            if (OfflineMode)
            {
                Logger.DebugError("Cannot connect, starting in offline mode, try restart the game");
                return;
            }

            
            await Task.Delay(2000);
        }

        public void JoinLobby()
        {
            PhotonNetwork.JoinLobby();
        }

        public void JoinRoom(string roomName = null)
        {
            if (string.IsNullOrEmpty(roomName))
            {
                PhotonNetwork.JoinRandomOrCreateRoom();
                return;
            }
            
            PhotonNetwork.JoinRoom(roomName);
        }

        public void CreateRoom(string roomName, string password = null, string matchTime = null)
        {
            RoomCallbacksDispatcher.OnCreatedRoomEvent += () =>
            {
                //Hashtable roomProperties = new Hashtable();
                //roomProperties["P"] = password;
                //roomProperties["T"] = matchTime;
                //PhotonNetwork.CurrentRoom.SetCustomProperties(roomProperties);
                //PhotonNetwork
            };
            RoomCallbacksDispatcher.OnPlayerEnteredRoomEvent += TryLoadGame;
            
            RoomOptions roomOptions = new RoomOptions()
            {
                MaxPlayers = 2,
                CustomRoomProperties = new Hashtable()
                {
                    ["P"] = password,
                    ["T"] = matchTime,
                },
                CustomRoomPropertiesForLobby = new[] {"P", "T"},
            };

            PhotonNetwork.CreateRoom(roomName, roomOptions, TypedLobby.Default);
            
            void TryLoadGame(Player newPlayer)
            {
                if (PhotonNetwork.CurrentRoom.PlayerCount == PhotonNetwork.CurrentRoom.MaxPlayers)
                {
                    PhotonNetwork.CurrentRoom.IsOpen = false;
                    Logger.DebugError("Load chess");
                    PhotonNetwork.LoadLevel(1);
                }
            }
        }

        private void OnDestroy()
        {
            PhotonNetwork.RemoveCallbackTarget(this);
        }

      
        #region ConnectionCallbacks

        public void OnConnected()
        {
            Logger.Debug("Established low level connection");
        }

        public void OnConnectedToMaster()
        {
            Logger.DebugError("Client connected to master");
        }

        public void OnDisconnected(DisconnectCause cause)
        {
            Logger.DebugError($"Client was disconnected {cause.ToString()}");
        }

        public void OnRegionListReceived(RegionHandler regionHandler)
        {
            Logger.Debug($"{regionHandler.GetResults()}");
        }

        public void OnCustomAuthenticationResponse(Dictionary<string, object> data)
        {
        }

        public void OnCustomAuthenticationFailed(string debugMessage)
        {
        }

        #endregion
    }
}