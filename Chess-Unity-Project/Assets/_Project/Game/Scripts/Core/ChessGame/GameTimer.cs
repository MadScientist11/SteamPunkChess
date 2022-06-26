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
        private Action<Team> _onTimerEnd;

        public GameTimer(TimerTextW whiteTimerText, TimerTextB blackTimerText)
        {
            _whiteTimerText = whiteTimerText.Text;
            _blackTimerText = blackTimerText.Text;
        }
        
        public void InitializeTimer(ChessPlayer whitePlayer, ChessPlayer blackPlayer, Action<Team> onTimeEnds)
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
                _whiteTimerText.text = ParseTime(_whitePlayer.PlayerRemainingTime);
                _blackTimerText.text = ParseTime(_blackPlayer.PlayerRemainingTime);
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
                        CurrentText.text = ParseTime(_currentPlayer.PlayerRemainingTime);
                    }
                    else
                    {
                        _currentPlayer.PlayerRemainingTime = 0;
                        _timerIsRunning = false;
                        _onTimerEnd?.Invoke(_currentPlayer.Team);
                        Logger.DebugError("On Timer end invoke");
                    }
                }
                yield return null;
            }
        }
        
        private string ParseTime(float time)
        {
            TimeSpan fromSeconds = TimeSpan.FromSeconds(time);
            ;
            if (time > 60)
                return $"{fromSeconds.Minutes:00}:{fromSeconds.Seconds:00}";
            else if (time < 60 && time > 15)
            {
                return fromSeconds.ToString(@"mm\:ss\:ff");
            }
            else
            {
                return fromSeconds.ToString(@"mm\:ss\:fff");
            }
        }

      
        
    }
}