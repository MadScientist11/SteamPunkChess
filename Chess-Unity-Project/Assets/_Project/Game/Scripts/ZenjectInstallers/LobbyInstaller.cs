using UnityEngine;
using Zenject;

namespace SteampunkChess
{
    public class LobbyInstaller : MonoInstaller
    {
        [SerializeField] private RoomListingMenu _roomListingMenu;
        [SerializeField] private RoomListingEntry _roomListingEntryPrefab;
        
        public override void InstallBindings()
        {
            BindRoomListingMenu();
            BindLobby();
            BindRoomListingEntryPrefab();
            BindRoomListingFactory();
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
                .AsSingle();
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
