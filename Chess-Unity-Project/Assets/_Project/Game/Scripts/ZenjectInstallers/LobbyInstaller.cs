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
            Container
                .Bind<RoomListingMenu>()
                .FromInstance(_roomListingMenu)
                .AsSingle();

            Container
                .BindInterfacesAndSelfTo<Lobby>()
                .AsSingle();
            
            Container
                .Bind<RoomListingEntry>()
                .FromInstance(_roomListingEntryPrefab)
                .AsSingle();

            Container
                .Bind<RoomListingEntryFactory>()
                .AsSingle();
        }
    }
}
