using System;
using SteampunkChess.NetworkService;
using TMPro;
using UnityEngine;
using Zenject;

namespace SteampunkChess.PopUps
{
    public enum MatchResult
    {
        Win,
        Lose,
        Draw
    }
    public class WinOrLosePopUp : PopUp
    {
        [Header("WinOrLosePopUp")]
        [SerializeField] private TextMeshProUGUI _winOrLoseText;
        [SerializeField] private TextMeshProUGUI _matchResultText;
        [SerializeField] private TextMeshProUGUI[] _playersTexts;
        [SerializeField] private TextMeshProUGUI _scoreText;
        
        private INetworkService _networkService;
        private IAudioSystem _audioSystem;

        public override string PopUpKey { get; set; } = GameConstants.PopUps.WinOrLoseWindow;

        [Inject]
        private void Construct(INetworkService networkService, IAudioSystem audioSystem)
        {
            _audioSystem = audioSystem;
            _networkService = networkService;
        }
        
        public override void Show(params object[] data)
        {
            base.Show(data);
            
            MatchResult matchResult = (MatchResult) data[0];
            switch (matchResult)
            {
                case MatchResult.Win:
                    _audioSystem.PlaySound(Sounds.WinSound);
                    DisplayAsWinPopUp();
                    break;
                case MatchResult.Lose:
                    _audioSystem.PlaySound(Sounds.LoseSound);
                    DisplayAsLosePopUp();
                    break;
                case MatchResult.Draw:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            
        }

        public void ToLobby()
        {
            _networkService.LeaveRoom();
        }

        private void DisplayAsWinPopUp()
        {
            _winOrLoseText.text = "Win";
            
            for (int i = 0; i < _playersTexts.Length; i++)
            {
                _playersTexts[i].text = _networkService.PlayersInfo[i].PlayerName;
                
                if(_networkService.LocalPlayer.PlayerID == _networkService.PlayersInfo[i].PlayerID)
                    _matchResultText.text = i == 0 ? "0:1" : "1:0";
            }

            _scoreText.text = _networkService.LocalPlayer.PlayerScore.ToString();
        }

        private void DisplayAsLosePopUp()
        {
            _winOrLoseText.text = "Lose";
            
            for (int i = 0; i < _playersTexts.Length; i++)
            {
                _playersTexts[i].text = _networkService.PlayersInfo[i].PlayerName;
                
                if(_networkService.LocalPlayer.PlayerID == _networkService.PlayersInfo[i].PlayerID)
                    _matchResultText.text = i == 0 ? "1:0" : "0:1";
            }

            _scoreText.text = _networkService.LocalPlayer.PlayerScore.ToString();
        }
    }
}
