using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using Hashtable = ExitGames.Client.Photon.Hashtable;

namespace SteampunkChess
{
    public class RoomCreationWindow : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _roomNameText;
        [SerializeField] private TextMeshProUGUI _passwordText;

        public void OnClick_CreateRoom()
        {
            PhotonNetwork.CreateRoom(_roomNameText.text, new RoomOptions
            {
                MaxPlayers = 2,
                CustomRoomProperties = new Hashtable()
                {
                    ["P"] = _passwordText.text
                }
            }, TypedLobby.Default);
        }
     
    }
}
