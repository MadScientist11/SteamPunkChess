using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using Zenject;
using Hashtable = ExitGames.Client.Photon.Hashtable;

namespace SteampunkChess.NetworkService
{
    [CreateAssetMenu(fileName = "PhotonServiceSO", menuName = "Services/PhotonServiceSO")]
    public class PhotonServiceSO : ScriptableObject, INetworkService, IConnectionCallbacks
    {
        public string InitializationMessage => "Connecting to the server...";

        private LobbyCallbacksDispatcher _lobbyCallbacksDispatcher;
        private RoomCallbacksDispatcher _roomCallbacksDispatcher;
        private PhotonRPCSender _photonRPCSender;
        private PlayFabPlayerData _playFabPlayerData;
        private const int GameSceneIndex = 1;

        public event Action<(string playerName, string playerScore)[]> OnReadyToStartGame;

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

                if (!_roomCallbacksDispatcher.gameObject.activeSelf)
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
            public int PlayerID => PhotonNetwork.LocalPlayer.ActorNumber;
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

                    return (int) PhotonNetwork.LocalPlayer.CustomProperties[GameConstants.CustomProperties.Team];
                }
                set
                {
                    Hashtable properties = new Hashtable()
                    {
                        [GameConstants.CustomProperties.Team] = value
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

                    return (int) PhotonNetwork.CurrentRoom.CustomProperties[GameConstants.CustomProperties.MatchTime];
                }
            }

            public bool IsMasterClient => PhotonNetwork.IsMasterClient;

            public int PlayerScore
            {
                get
                {
                    return (int) PhotonNetwork.LocalPlayer.CustomProperties[GameConstants.CustomProperties.Score];
                }
                set
                {
                    Hashtable properties = new Hashtable()
                    {
                        [GameConstants.CustomProperties.Score] = value
                    };
                    PhotonNetwork.LocalPlayer.SetCustomProperties(properties);
                }
            }
        }

        public NetworkPlayer LocalPlayer { get; private set; }

        public List<PlayerInfoDTO>  PlayersInfo
        {
            get
            {
                if (_playersInfo != null) return _playersInfo;
                
                _playersInfo = new List<PlayerInfoDTO>();
            
                foreach (var player in PhotonNetwork.PlayerList)
                {
                    int score = (int) player.CustomProperties[GameConstants.CustomProperties.Score];
                    int team = (int) player.CustomProperties[GameConstants.CustomProperties.Team];
                    _playersInfo.Add(
                        new PlayerInfoDTO(player.ActorNumber, player.NickName, score, (Team)team)
                    );
                }

                return _playersInfo;
            }
        }

        private List<PlayerInfoDTO> _playersInfo;

        public event Action OnConnectedToMasterEvent;

        [Inject]
        private void Construct(ServiceContainer serviceContainer, PlayFabPlayerData playFabPlayerData)
        {
            _playFabPlayerData = playFabPlayerData;
            serviceContainer.ServiceList.Add(this);
        }

        public async Task Initialize()
        {
            PhotonNetwork.AddCallbackTarget(this);

            PhotonNetwork.ConnectUsingSettings();
            LocalPlayer = new NetworkPlayer();
            
            _playFabPlayerData.OnPlayerDataChanged += _playerData =>
            {
                LocalPlayer.PlayerName = _playerData.PlayerName;
                LocalPlayer.PlayerScore = _playerData.PlayerScore;
            };
            
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


            PhotonNetwork.JoinRoom(roomName);
        }

        public void LeaveRoom()
        {
            PhotonNetwork.LeaveRoom();
            RoomCallbacksDispatcher.ClearRoomCallbackEvents();
            LobbyCallbacksDispatcher.ClearLobbyCallbacksEvents();
        }

        public void CreateRoom(string roomName, int timeLimitInSeconds, int playerTeam, string password = null)
        {
            LocalPlayer.PlayerTeam = playerTeam;

            RoomCallbacksDispatcher.OnPlayerEnteredRoomEvent += SetEnteredPlayerTeamAndCloseGame;
            

            RoomOptions roomOptions = new RoomOptions()
            {
                MaxPlayers = 2,
                CustomRoomProperties = new Hashtable()
                {
                    [GameConstants.CustomProperties.RoomPassword] = password,
                    [GameConstants.CustomProperties.MatchTime] = timeLimitInSeconds,
                },
                CustomRoomPropertiesForLobby = new[] {GameConstants.CustomProperties.RoomPassword, GameConstants.CustomProperties.MatchTime},
            };

            PhotonNetwork.CreateRoom(roomName, roomOptions, TypedLobby.Default);

            void SetEnteredPlayerTeamAndCloseGame(Player newPlayer)
            {
                Hashtable teamProperty = new Hashtable()
                {
                    [GameConstants.CustomProperties.Team] = LocalPlayer.PlayerTeam == 0 ? 1 : 0
                };
                newPlayer.SetCustomProperties(teamProperty);


                if (PhotonNetwork.CurrentRoom.PlayerCount == PhotonNetwork.CurrentRoom.MaxPlayers)
                {
                    PhotonNetwork.CurrentRoom.IsOpen = false;
                    PhotonNetwork.CurrentRoom.IsVisible = false;
                }
            }

            
        }

        public void LoadGame()
        {
            if (PhotonNetwork.IsMasterClient)
                PhotonNetwork.LoadLevel(GameSceneIndex);
            
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
            OnConnectedToMasterEvent?.Invoke();
            Logger.DebugError("Client connected to master");
            OnConnectedToMasterEvent = null;
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