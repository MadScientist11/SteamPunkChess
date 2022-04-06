using System;
using System.Collections;
using System.Collections.Generic;
using SteampunkChess.NetworkService;
using SteampunkChess.PopUps;
using SteampunkChess.PopUpService;
using TMPro;
using UnityEngine;
using Zenject;

namespace SteampunkChess
{
    public class PlayersOverviewPopUp : PopUp
    {
        [SerializeField] private GameObject _popUpGO;
        
        [SerializeField] private TextMeshProUGUI[] _playersNameTexts;
        [SerializeField] private TextMeshProUGUI[] _playersScoreTexts;
        
        [SerializeField] private TextMeshProUGUI _countdownText;
        private float _countDownTime = 5f;
        private IPopUpService _popUpService;
        private INetworkService _networkService;

        [Inject]
        private void Construct(IPopUpService popUpService, INetworkService networkService)
        {
            _networkService = networkService;
            _popUpService = popUpService;
        }
       
        public override void Start()
        {
            
        }

        private void StartCountdown_AnimationEvent()
        {
            StartCoroutine(StartCountdownToStartGame());
        }

        public override void Show(params object[] data)
        {
            var playerInfo = (List<PlayerInfoDTO>) data[0];
            InitializePlayersUI(playerInfo);
            _popUpGO.SetActive(true);
        }

        private void InitializePlayersUI(List<PlayerInfoDTO> playerInfo)
        {
            for (int i = 0; i < 2; i++)
            {
                _playersNameTexts[i].text = playerInfo[i].PlayerName;
                _playersScoreTexts[i].text = playerInfo[i].PlayerScore.ToString();
            }
        }

        private IEnumerator StartCountdownToStartGame()
        {
            float remainingTime = _countDownTime;
            
            while (true)
            {
                if (remainingTime > 0)
                {
                    remainingTime -= Time.deltaTime; 
                    TimeSpan time = TimeSpan.FromSeconds(remainingTime);
                    _countdownText.text = $"{time.Seconds}";
                    
                }
                else
                {
                    StopCoroutine(StartCountdownToStartGame());
                    _networkService.LoadGame();
                    _popUpService.HidePopUp(GameConstants.PopUps.PlayersOverviewWindow, HideType.HideDestroyAndRelease);
                    yield break;
                }

                yield return null;

            }
        }
    }
}
