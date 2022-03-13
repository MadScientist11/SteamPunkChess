using UnityEngine;

namespace SteamPunkChess
{
    interface IInputHandler<T>
    {
        void ProcessInput(T input);
    }
}