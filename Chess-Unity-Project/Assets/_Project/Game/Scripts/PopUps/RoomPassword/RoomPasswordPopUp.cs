using SteampunkChess.NetworkService;
using SteampunkChess.PopUps;
using SteampunkChess.PopUpService;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace SteampunkChess
{
    public class RoomPasswordPopUp : PopUp
    {
        [SerializeField] private TMP_InputField _passwordInputField;
        [SerializeField] private Button _joinButton;
        private string _roomPassword;
        private INetworkService _networkService;
        private IPopUpService _popUpService;

        [Inject]
        private void Construct(INetworkService networkService, IPopUpService popUpService)
        {
            _popUpService = popUpService;
            _networkService = networkService;
        }
        
        public override void Show(params object[] data)
        {
            base.Show(data);
            _roomPassword = (string)data[0];
        }

        public void JoinRoom()
        {
            if (_passwordInputField.text == _roomPassword)
            {
                _networkService.JoinRoom();
            }
            else
            {
                _popUpService.ShowPopUp(GameConstants.PopUps.ErrorToast, "Incorrect password");
                _popUpService.HidePopUp(GameConstants.PopUps.RoomPasswordWindow, HideType.Hide);
            }
        }
    }
}