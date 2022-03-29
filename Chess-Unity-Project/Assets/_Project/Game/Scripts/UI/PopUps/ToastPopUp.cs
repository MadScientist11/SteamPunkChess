using System.Threading.Tasks;
using SteampunkChess.PopUps;
using SteampunkChess.PopUpService;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

namespace SteampunkChess
{
    public class ToastPopUp : PopUp, IPointerClickHandler
    {
        [SerializeField] private TextMeshProUGUI _toastText;
        [SerializeField] private int _popUpLifetimeMs = 5000;
        [SerializeField] private string _addressableToastName;
        private string _message;
        
        private IPopUpService _popUpService;

        [Inject]
        public void Construct(IPopUpService popUpService)
        {
            _popUpService = popUpService;
        }

        public override async void Start()
        {
            base.Start();
            await Task.Delay(_popUpLifetimeMs);
            _popUpService.HidePopUp(_addressableToastName, HideType.HideDestroyAndRelease);
        }


        public override void Show(params object[] data)
        {
            base.Show(data);
            _message = (string) data[0];
            _toastText.text = _message;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            _popUpService.HidePopUp(_addressableToastName, HideType.HideDestroyAndRelease);
        }
    }
}
