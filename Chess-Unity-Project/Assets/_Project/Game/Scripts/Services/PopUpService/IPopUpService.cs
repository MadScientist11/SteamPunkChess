using SteampunkChess.PopUps;

namespace SteampunkChess.PopUpService
{
    public interface IPopUpService : IService
    {
        void ShowPopUp(string popUpKey, params object[] data);
        
        void HidePopUp(string popUpKey, HideType hideType);
        void HideAll(HideType hideType);

        void RemoveInstanceFromInternalPool(string instanceKey);

    }
}