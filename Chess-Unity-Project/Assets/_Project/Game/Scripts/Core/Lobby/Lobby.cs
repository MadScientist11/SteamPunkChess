using System.Collections.Generic;
using ExitGames.Client.Photon;
using ModestTree;
using Photon.Pun;
using Photon.Realtime;
using SteampunkChess.NetworkService;
using SteampunkChess.PopUpService;

namespace SteampunkChess
{
    public class RoomData
    {
        public string RoomName;
        public string Password;
        public int TimeLimitInSeconds;
    }

    public class Lobby : IInitializable
    {
        private readonly RoomListingMenu _roomListingMenu;
        private readonly LobbyUI _lobbyUI;
        private readonly INetworkService _networkService;
        private readonly IPopUpService _popUpService;

        public Lobby(INetworkService networkService, IPopUpService popUpService, RoomListingMenu roomListingMenu,
            LobbyUI lobbyUI)
        {
            _popUpService = popUpService;
            _networkService = networkService;
            _roomListingMenu = roomListingMenu;
            _lobbyUI = lobbyUI;
        }

        public void Initialize()
        {
            if (PhotonNetwork.IsConnectedAndReady)
            {
                InitializeLobby();
            }
            else
            {
                PhotonNetwork.ConnectUsingSettings();
                _networkService.OnConnectedToMasterEvent += InitializeLobby;
            }
        }

        private void InitializeLobby()
        {
            _networkService.AutomaticallySyncScene = true;
            
            _networkService.LobbyCallbacksDispatcher.OnRoomListUpdateEvent += _roomListingMenu.UpdateRoomListing;
            _networkService.RoomCallbacksDispatcher.OnPlayerPropertiesUpdateEvent += TryStartGame;
            _networkService.RoomCallbacksDispatcher.OnCreatedRoomEvent += ShowLoadingScreen;
            
            _networkService.JoinLobby();
        }

        private void ShowStartingGamePopUp(List<PlayerInfoDTO> playersInfo)
        {
            _lobbyUI.Disable();
            _popUpService.ShowPopUp(GameConstants.PopUps.PlayersOverviewWindow, playersInfo);
        }

        private void TryStartGame(Player newPlayer, Hashtable props)
        {
            if (!newPlayer.IsMasterClient && props[GameConstants.CustomProperties.Team] != null)
            {
                ShowStartingGamePopUp(_networkService.PlayersInfo);
            }
        }

        private void ShowLoadingScreen()
        {
            //loading and waiting for opponent
            _lobbyUI.SwitchToLoading();
        }

        public void JoinRoom()
        {
            _networkService.JoinRoom();
        }
    }
}