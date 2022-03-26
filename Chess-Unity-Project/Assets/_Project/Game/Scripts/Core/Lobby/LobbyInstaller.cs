using UnityEngine;
using Zenject;

namespace SteampunkChess
{
    public class LobbyInstaller : MonoInstaller
    {
        [SerializeField] private RoomListingMenu _roomListingMenu;
        
        public override void InstallBindings()
        {
            Container
                .Bind<RoomListingMenu>()
                .FromInstance(_roomListingMenu)
                .AsSingle();

            Container
                .BindInterfacesAndSelfTo<Lobby>()
                .AsSingle();
        }
    }
}
