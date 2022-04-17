using System;
namespace SteampunkChess.PopUps
{
    public interface IPopUp
    { 
        string PopUpKey { get; set; }

        bool IsVisible { get; }
        public event Action OnDestroyed;
        void Show(params object[] data);
        void Hide(bool destroyAfterHide = false);
    }
}