using UnityEngine;
using Zenject;

namespace SteampunkChess
{
    public class LobbyEntryPoint : MonoBehaviour
    {
        private Lobby _lobby;
        
        [Inject]
        private void Construct(Lobby lobby)
        {
            _lobby = lobby;
        }

        private void Awake()
        {
            _lobby.Initialize();
        }
    }
}
