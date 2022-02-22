using System;
namespace SteampunkChess.PopUps
{
    public interface IPopUp
    {
        public event Action OnDestroyed;
        void Show();
        void Hide(bool destroyAfterHide = false);
    }
}