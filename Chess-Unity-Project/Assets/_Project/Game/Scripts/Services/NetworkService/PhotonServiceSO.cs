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
    [CreateAssetMenu(fileName = "PhotonServiceSO", menuName = "Services/PhotonServiceSO")]
    public class PhotonServiceSO : ScriptableObject, INetworkService, IConnectionCallbacks
    {
        public string InitializationMessage => "Connecting to the server...";
        
        private LobbyCallbacksDispatcher _lobbyCallbacksDispatcher;
        private RoomCallbacksDispatcher _roomCallbacksDispatcher;
        private PhotonRPCSender _photonRPCSender;

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
        
        public class NetworkPlayer
        {
            public string PlayerName
            {
                get => PhotonNetwork.NickName;
                set => PhotonNetwork.NickName = value;
            }

            public int PlayerTeam
            {
                get
                {
                    if (!PhotonNetwork.InRoom)
                        throw new Exception("Cannot address PlayerTeam property while not in a room");
                    
                    return (int) PhotonNetwork.LocalPlayer.CustomProperties["Team"];
                }
                set
                {
                    Hashtable properties = new Hashtable()
                    {
                        ["Team"] = value
                    };
                    PhotonNetwork.LocalPlayer.SetCustomProperties(properties);
                }
            }

            public int MatchTimeLimitInSeconds
            {
                get
                {
                    if (!PhotonNetwork.InRoom)
                        throw new Exception("Cannot address MatchTimeLimit property while not in a room");
                    
                    return (int) PhotonNetwork.CurrentRoom.CustomProperties["T"];
                }
            }

        }

        public NetworkPlayer LocalPlayer { get; private set; }

        [Inject]
        private void Construct(ServiceContainer serviceContainer)
        {
            serviceContainer.ServiceList.Add(this);
        }

        public async Task Initialize()
        {
            PhotonNetwork.AddCallbackTarget(this);
            
            PhotonNetwork.ConnectUsingSettings();
            LocalPlayer = new NetworkPlayer();

            if (OfflineMode)
            {
                Logger.DebugError("Cannot connect, starting in offline mode, try restart the game");
                return;
            }

            
            await Task.Delay(000);
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
            
            LocalPlayer.PlayerTeam = 1;
            
            PhotonNetwork.JoinRoom(roomName);
        }

        public void CreateRoom(string roomName, string password = null, int timeLimitInSeconds = -1)
        {
            RoomCallbacksDispatcher.OnCreatedRoomEvent += () =>
            {
                //Hashtable roomProperties = new Hashtable();
                //roomProperties["P"] = password;
                //roomProperties["T"] = matchTime;
                //PhotonNetwork.CurrentRoom.SetCustomProperties(roomProperties);
                //PhotonNetwork
            };

            LocalPlayer.PlayerTeam = 0;
            
            RoomCallbacksDispatcher.OnPlayerEnteredRoomEvent += TryLoadGame;
            
            RoomOptions roomOptions = new RoomOptions()
            {
                MaxPlayers = 2,
                CustomRoomProperties = new Hashtable()
                {
                    ["P"] = password,
                    ["T"] = timeLimitInSeconds,
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