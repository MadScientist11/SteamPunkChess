using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using Zenject;

namespace SteampunkChess.NetworkService
{
    public interface INetworkService : IService
    {
        bool OfflineMode { get; }
        
        string Username { get; set; }

        public bool AutomaticallySyncScene { set; }
        public LobbyCallbacksDispatcher LobbyCallbacksDispatcher { get; }
        public RoomCallbacksDispatcher RoomCallbacksDispatcher { get; }
        void SendRPC(string methodName, RpcTarget target, params object[] parameters);
        void CreateRoom(string roomName, string password = null, string matchTime = null, RoomOptions roomOptions = null, TypedLobby typedLobby = null);
        void JoinLobby();
        void JoinRoom(string roomName = null);
    }

    [RequireComponent(typeof(PhotonView))]
    public class PhotonRPCSender : MonoBehaviour
    {
        private PhotonView _photonView;

        private void Awake()
        {
            _photonView = GetComponent<PhotonView>();
        }

        public void SendRPC(string methodName, RpcTarget target, params object[] parameters)
        {
            _photonView.RPC(methodName, target, parameters);
        }
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
        private void OnEnable()
        {
            PhotonNetwork.AddCallbackTarget(this);
        }

        private void OnDisable()
        {
            PhotonNetwork.RemoveCallbackTarget(this);
        }

        #region InRoomCallbacks
        
        public void OnPlayerEnteredRoom(Player newPlayer)
        {
            OnPlayerEnteredRoomEvent?.Invoke(newPlayer);
            OnPlayerEnteredRoomEvent = null;
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
            OnCreatedRoomEvent = null;
        }

        public void OnCreateRoomFailed(short returnCode, string message)
        {
          
        }

        public void OnJoinedRoom()
        {
            OnJoinedRoomEvent?.Invoke();
        }

        public void OnJoinRoomFailed(short returnCode, string message)
        {
           
        }

        public void OnJoinRandomFailed(short returnCode, string message)
        {
           
        }

        public void OnLeftRoom()
        {
            
        }
        #endregion
       
    }


    

    [CreateAssetMenu(fileName = "PhotonServiceSO", menuName = "Services/PhotonServiceSO")]
    public class PhotonServiceSO : ScriptableObject, INetworkService, IConnectionCallbacks
    {
        public string InitializationMessage => "Connecting to the server...";
        
        private PhotonRPCSender _rpcSender;
        public LobbyCallbacksDispatcher LobbyCallbacksDispatcher { get; private set; }
        public RoomCallbacksDispatcher RoomCallbacksDispatcher { get; private set; }
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
            
            _rpcSender = new GameObject("RpcSender")
                .AddComponent<PhotonRPCSender>();
            DontDestroyOnLoad(_rpcSender);
           
            await Task.Delay(2000);
        }

        public void JoinLobby()
        {
            if (LobbyCallbacksDispatcher == null)
            {
                LobbyCallbacksDispatcher = new GameObject("LobbyCallbacksDispatcher")
                    .AddComponent<LobbyCallbacksDispatcher>();
                DontDestroyOnLoad(LobbyCallbacksDispatcher);
            }

            PhotonNetwork.JoinLobby();
        }

        public void JoinRoom(string roomName = null)
        {
            if (RoomCallbacksDispatcher == null)
            {
                RoomCallbacksDispatcher = new GameObject("RoomCallbacksDispatcher")
                    .AddComponent<RoomCallbacksDispatcher>();
                DontDestroyOnLoad(RoomCallbacksDispatcher);
            }

            if (string.IsNullOrEmpty(roomName))
            {
                PhotonNetwork.JoinRandomRoom();
                return;
            }

            RoomCallbacksDispatcher.OnPlayerEnteredRoomEvent += (_) =>
            {
                if (PhotonNetwork.CurrentRoom.PlayerCount == PhotonNetwork.CurrentRoom.MaxPlayers)
                {
                    PhotonNetwork.CurrentRoom.IsOpen = false;
                    PhotonNetwork.LoadLevel(1);
                }
            };

            PhotonNetwork.JoinRoom(roomName);
        }

        public void CreateRoom(string roomName, string password = null, string matchTime = null, RoomOptions roomOptions = null, TypedLobby typedLobby = null)
        {
            if (RoomCallbacksDispatcher == null)
            {
                RoomCallbacksDispatcher = new GameObject("RoomCallbacksDispatcher")
                    .AddComponent<RoomCallbacksDispatcher>();
                DontDestroyOnLoad(RoomCallbacksDispatcher);
            }

            RoomCallbacksDispatcher.OnCreatedRoomEvent += () =>
            {
                Hashtable roomProperties = new Hashtable();
                roomProperties["P"] = password;
                roomProperties["T"] = matchTime;
                PhotonNetwork.CurrentRoom.SetCustomProperties(roomProperties);
            };
            
            PhotonNetwork.CreateRoom(roomName, roomOptions, typedLobby);
        }
        
        
        
        private void OnDestroy()
        {
            PhotonNetwork.RemoveCallbackTarget(this);
        }

        public void SendRPC(string methodName, RpcTarget target, params object[] parameters)
        {
            _rpcSender.SendRPC(methodName, target, parameters);
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