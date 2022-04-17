using System;
using UnityEngine;

namespace SteampunkChess
{
    public interface IInputSystem : IService
    {
        public bool CheckForInput { get; }

        public Action OnBackButtonPressed { get; set; }
        public Action<KeyCode> OnCameraViewChanged { get; set; }
    }
}