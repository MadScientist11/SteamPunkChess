using System;
using System.Collections;
using TMPro;
using UnityEngine;

namespace SteampunkChess
{
    public class GameTimer 
    {
        private const int NoLimitInTime = 0;
        private readonly TextMeshProUGUI _whiteTimerText;
        private readonly TextMeshProUGUI _blackTimerText;
        
        private bool _timerIsRunning;

        private TextMeshProUGUI CurrentText => _currentPlayer.Team == Team.White ? _whiteTimerText : _blackTimerText;
        private ChessPlayer _currentPlayer;
        private ChessPlayer _whitePlayer;
        private ChessPlayer _blackPlayer;
        private Action _onTimerEnd;

        public GameTimer(TimerTextW whiteTimerText, TimerTextB blackTimerText)
        {
            _whiteTimerText = whiteTimerText.Text;
            _blackTimerText = blackTimerText.Text;
        }
        
        public void InitializeTimer(ChessPlayer whitePlayer, ChessPlayer blackPlayer, Action onTimeEnds)
        {
            _onTimerEnd = onTimeEnds;
            _whitePlayer = whitePlayer;
            _blackPlayer = blackPlayer;
            _currentPlayer = whitePlayer;
            
            if (_currentPlayer.PlayerRemainingTime == NoLimitInTime)
            {
                _whiteTimerText.text = "∞";
                _blackTimerText.text = "∞";
            }
            else
            {
                CoroutineStarter.Instance.StartCoroutine(Tick());
            }
        }

        public void Start()
        {
            _timerIsRunning = true;
        }

        public void Stop()
        {
            _timerIsRunning = false;
            CoroutineStarter.Instance.StopCoroutine(Tick());
        }

        public void SwitchPlayer()
        {
            _currentPlayer = _currentPlayer.Team == _whitePlayer.Team ? _blackPlayer : _whitePlayer;
        }
        
        private IEnumerator Tick()
        {
            while (true)
            {
                if(_timerIsRunning)
                {
                    if (_currentPlayer.PlayerRemainingTime > 0)
                    {
                        _currentPlayer.PlayerRemainingTime -= Time.deltaTime;
                        DisplayTime(_currentPlayer.PlayerRemainingTime);
                    }
                    else
                    {
                        _currentPlayer.PlayerRemainingTime = 0;
                        _timerIsRunning = false;
                        _onTimerEnd?.Invoke();
                    }
                }
                yield return null;
            }
        }
        
        private void DisplayTime(float time)
        {
            TimeSpan fromSeconds = TimeSpan.FromSeconds(time);
            Logger.DebugError("Display time");
            CurrentText.text = time > 60 ? $"{fromSeconds.Minutes:00}:{fromSeconds.Seconds:00}" : fromSeconds.ToString(@"mm\:ss\:fff");
        }

      
        
    }
}