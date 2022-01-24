using SteampunkChess.PopUps;

namespace SteampunkChess.PopUpService
{
    public interface IPopUpService
    {
        void ShowPopUp(string popUpKey);
        void HidePopUp(string popUpKey, HideType hideType);

    }
}