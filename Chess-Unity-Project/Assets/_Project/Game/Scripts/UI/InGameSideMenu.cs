using System;
using System.Collections.Generic;
using SteampunkChess.NetworkService;
using TMPro;
using UnityEngine;
using Zenject;

namespace SteampunkChess
{

    [Serializable]
    public class PlayerUI
    {
        public TextMeshProUGUI scoreText;
        public TextMeshProUGUI playerNameText;
    }
    
    public class InGameSideMenu : MonoBehaviour
    {
        [SerializeField] private PlayerUI[] _playersUI;
        private INetworkService _networkService;

        [Inject]
        private void Construct(INetworkService networkService)
        {
            _networkService = networkService;
        }

        private void Start()
        {
            InitializePlayersUI(_networkService.PlayersInfo);
        }
        
        private void InitializePlayersUI(List<PlayerInfoDTO> playerInfo)
        {
            PlayerInfoDTO whitePlayer = playerInfo[0].PlayerTeam == Team.White ? playerInfo[0] : playerInfo[1];
            PlayerInfoDTO blackPlayer = playerInfo[0].PlayerTeam == Team.Black ? playerInfo[0] : playerInfo[1];

            _playersUI[0].playerNameText.text = whitePlayer.PlayerName;
            _playersUI[0].scoreText.text = whitePlayer.PlayerScore.ToString();
            _playersUI[1].playerNameText.text = blackPlayer.PlayerName;
            _playersUI[1].scoreText.text = blackPlayer.PlayerScore.ToString();
        }
    }
}
