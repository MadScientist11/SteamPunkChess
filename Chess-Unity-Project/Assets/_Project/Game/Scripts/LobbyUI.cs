using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SteampunkChess
{
    public class LobbyUI : MonoBehaviour
    {
        [SerializeField] private GameObject _loadingUI;
        public void Disable()
        {
            gameObject.SetActive(false);
        }

        public void SwitchToLoading()
        {
            Disable();
            _loadingUI.SetActive(true);
        }
    }
}
