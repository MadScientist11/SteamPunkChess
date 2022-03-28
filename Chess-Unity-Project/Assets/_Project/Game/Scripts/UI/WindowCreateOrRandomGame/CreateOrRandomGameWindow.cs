using Photon.Realtime;
using UnityEngine;
using Zenject;

namespace SteampunkChess
{
    public class CreateOrRandomGameWindow : MonoBehaviour
    {
        private Lobby _lobby;

        [Inject]
        private void Construct(Lobby lobby)
        {
            _lobby = lobby;
        }

        public void CreateRoom()
        {
            //show room creation window
            RoomData roomData = new RoomData()
            {
                RoomName = "TestRoom",
                Password = "",
                Time = "5:00",
            };
            _lobby.CreateRoom(roomData);
        }

        public void JoinRandomGame()
        {
            _lobby.JoinRoom();
        }
    }
}
