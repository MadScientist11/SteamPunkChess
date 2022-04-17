using UnityEngine;
using UnityEngine.Events;

namespace SteampunkChess.SignalSystem
{
    public class SignalListener : MonoBehaviour, ISignalListener
    {
        [SerializeField] private Signal _signal;
        [SerializeField] private UnityEvent _signalEvent;

        public void OnSignalRaised()
        {
            _signalEvent.Invoke();
        }
        private void OnEnable()
        {
            _signal.RegisterSignal(this);
        }
        private void OnDestroy()
        {
            _signal.UnregisterSignal(this);
        }
    }
}
