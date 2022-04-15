using SteampunkChess.NetworkService;
using SteampunkChess.PopUps;
using SteampunkChess.PopUpService;
using TMPro;
using UnityEngine;
using Zenject;

namespace SteampunkChess
{
    public class RoomPasswordPopUp : PopUp
    {
        [SerializeField] private TMP_InputField _passwordInputField;
        
        private string _roomPassword;
        private string _roomName;
        private INetworkService _networkService;
        private IPopUpService _popUpService;
        private IAudioSystem _audioSystem;

        public override string PopUpKey { get; set; } = GameConstants.PopUps.RoomPasswordWindow;

        [Inject]
        private void Construct(INetworkService networkService, IPopUpService popUpService, IAudioSystem audioSystem)
        {
            _audioSystem = audioSystem;
            _popUpService = popUpService;
            _networkService = networkService;
        }
        
        public override void Show(params object[] data)
        {
            base.Show(data);
            _roomName = (string)data[0];
            _roomPassword = (string)data[1];
        }

        public void JoinRoom()
        {
            _popUpService.HidePopUp(GameConstants.PopUps.RoomPasswordWindow, HideType.Hide);
            if (_passwordInputField.text == _roomPassword)
            {
                _networkService.JoinRoom(_roomName);
            }
            else
            {
                _audioSystem.PlaySound(Sounds.IncorrectPasswordSound);
                _popUpService.ShowPopUp(GameConstants.PopUps.ErrorToast, "Incorrect password");
                
            }
        }
    }
}