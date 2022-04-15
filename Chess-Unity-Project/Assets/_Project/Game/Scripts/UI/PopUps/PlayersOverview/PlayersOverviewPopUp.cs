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
        [Header("PlayersOverviewPopUp")]
        [SerializeField] private GameObject _popUpGO;
        
        [SerializeField] private TextMeshProUGUI[] _playersNameTexts;
        [SerializeField] private TextMeshProUGUI[] _playersScoreTexts;
        
        [SerializeField] private TextMeshProUGUI _countdownText;
        private const float _countDownTime = 5f;
        private IPopUpService _popUpService;
        private INetworkService _networkService;

        public override string PopUpKey { get; set; } = GameConstants.PopUps.PlayersOverviewWindow;

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
            PlayerInfoDTO whitePlayer = playerInfo[0].PlayerTeam == Team.White ? playerInfo[0] : playerInfo[1];
            PlayerInfoDTO blackPlayer = playerInfo[0].PlayerTeam == Team.Black ? playerInfo[0] : playerInfo[1];
            
            _playersNameTexts[0].text = whitePlayer.PlayerName;
            _playersScoreTexts[0].text = whitePlayer.PlayerScore.ToString();
            _playersNameTexts[1].text = blackPlayer.PlayerName;
            _playersScoreTexts[1].text = blackPlayer.PlayerScore.ToString();
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
                    _popUpService.HidePopUp(GameConstants.PopUps.PlayersOverviewWindow, HideType.HideAndDestroy);
                    _networkService.LoadGame();
                    yield break;
                }

                yield return null;
            }
        }
        
        
    }
}
