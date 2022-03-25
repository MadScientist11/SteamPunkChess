using Photon.Pun;
using Photon.Realtime;
using SteampunkChess.PopUpService;
using TMPro;
using UnityEngine;
using Zenject;

namespace SteampunkChess
{
    public class RoomListing : MonoBehaviour, IInitializable
    {
        [SerializeField] private TextMeshProUGUI _roomName;
        private IPopUpService _popUpService;
        public RoomInfo RoomInfo { get; set; }

        [Inject]
        private void Construct(IPopUpService popUpService)
        {
            _popUpService = popUpService;
        }
        
        public void Initialize()
        {
            _roomName.text = RoomInfo.Name;
        }

        public void OnClick_JoinRoom()
        {
            string roomPassword = (string)RoomInfo.CustomProperties["P"];
            if (!string.IsNullOrEmpty(roomPassword))
            {
                _popUpService.ShowPopUp(GameConstants.PopUps.RoomPasswordWindow, this);
                Debug.LogError("Need a password");
                return;
            }
            
            PhotonNetwork.JoinRoom(RoomInfo.Name);
        }

     
    }
}
