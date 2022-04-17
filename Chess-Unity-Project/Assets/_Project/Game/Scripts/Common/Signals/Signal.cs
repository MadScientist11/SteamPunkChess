using System.Collections.Generic;
using UnityEngine;

namespace SteampunkChess.SignalSystem
{
    [CreateAssetMenu(fileName = "Signal", menuName = "Signal")]
    public class Signal : ScriptableObject
    {
        public List<ISignalListener> Listeners = new List<ISignalListener>();

        public void Raise()
        {
            for (int i = 0; i < Listeners.Count; i++)
            {
                Listeners[i].OnSignalRaised();
            }
        }

        public void RegisterSignal(ISignalListener signalListener)
        {
            Listeners.Add(signalListener);
        }

        public void UnregisterSignal(ISignalListener signalListener)
        {
            Listeners.Remove(signalListener);
        }
    }
}
