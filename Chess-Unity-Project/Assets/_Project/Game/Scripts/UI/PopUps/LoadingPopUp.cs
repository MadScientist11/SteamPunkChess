using System.Collections;
using Photon.Pun;
using SteampunkChess.PopUps;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SteampunkChess
{
    public class LoadingPopUp : PopUp
    {
        [SerializeField] private TextMeshProUGUI _loadingText;
        [SerializeField] private Slider _slider;
        public override string PopUpKey { get; set; } = GameConstants.PopUps.LoadingPopUp;

        public override void Start()
        {
            
        }

        public override void Show(params object[] data)
        {
            StartCoroutine(LoadLevelAsync());
        }
        
        
        IEnumerator LoadLevelAsync()
        {
            while(PhotonNetwork.LevelLoadingProgress < 1f)
            {
                float progress = Mathf.Clamp01(PhotonNetwork.LevelLoadingProgress / .9f);
                Debug.LogError(progress);
                _slider.value = progress;
                yield return null;
            }        
        }
    }
}
