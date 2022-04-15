using UnityEngine;
using Zenject;

namespace SteampunkChess
{
    public class LobbyInstaller : MonoInstaller
    {
        [SerializeField] private RoomListingMenu _roomListingMenu;
        [SerializeField] private RoomListingEntry _roomListingEntryPrefab;
        [SerializeField] private LobbyUI _lobbyUI;
        
        public override void InstallBindings()
        {
            BindRoomListingMenu();
            BindLobby();
            BindRoomListingEntryPrefab();
            BindRoomListingFactory();
            Container
                .Bind<LobbyUI>()
                .FromInstance(_lobbyUI)
                .AsTransient();
        }

        private void BindRoomListingFactory()
        {
            Container
                .Bind<RoomListingEntryFactory>()
                .AsSingle();
        }

        private void BindRoomListingEntryPrefab()
        {
            Container
                .Bind<RoomListingEntry>()
                .FromInstance(_roomListingEntryPrefab)
                .AsSingle();
        }

        private void BindLobby()
        {
            Container
                .Bind<Lobby>()
                .AsTransient();
        }

        private void BindRoomListingMenu()
        {
            Container
                .Bind<RoomListingMenu>()
                .FromInstance(_roomListingMenu)
                .AsSingle();
        }
    }
}
