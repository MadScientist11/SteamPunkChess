using System;
using System.Collections;
using System.Collections.Generic;
using SteampunkChess.NetworkService;
using SteampunkChess.PopUps;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
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

        [Inject]
        private void Construct(INetworkService networkService)
        {
            _networkService = networkService;
        }
        
        public override void Show(params object[] data)
        {
            base.Show(data);
            MatchResult matchResult = (MatchResult) data[0];
            switch (matchResult)
            {
                case MatchResult.Win:
                    DisplayAsWinPopUp();
                    break;
                case MatchResult.Lose:
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
            Addressables.LoadSceneAsync("Lobby");
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
