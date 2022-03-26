using System;
using Photon.Pun;
using Photon.Realtime;
using SteampunkChess.NetworkService;
using SteampunkChess.PopUpService;
using TMPro;
using UnityEngine;
using Zenject;

namespace SteampunkChess
{
    public class RoomListingEntry : MonoBehaviour, IInitializable
    {
        [SerializeField] private TextMeshProUGUI _roomNameText;
        [SerializeField] private TextMeshProUGUI _matchTimeText;
        [SerializeField] private GameObject _passwordIcon;
        private string _roomPassword;
        private IPopUpService _popUpService;
        private INetworkService _networkService;
        public RoomInfo RoomInfo { get; set; }
        private bool IsRoomUnderPassword => !string.IsNullOrEmpty(_roomPassword);

        [Inject]
        private void Construct(IPopUpService popUpService, INetworkService networkService)
        {
            _networkService = networkService;
            _popUpService = popUpService;
        }
        
        public void Initialize()
        {
            _roomPassword = (string)RoomInfo.CustomProperties["P"];
            _roomNameText.text = RoomInfo.Name;
            _matchTimeText.text = (string)RoomInfo.CustomProperties["T"];
            if(IsRoomUnderPassword)
                _passwordIcon.SetActive(true);
        }
        
        public void JoinRoom()
        {
            if (IsRoomUnderPassword)
            {
                _popUpService.ShowPopUp(GameConstants.PopUps.RoomPasswordWindow, _roomPassword);
                return;
            }
            _networkService.JoinRoom(RoomInfo.Name);
        }
    }
}
