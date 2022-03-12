using UnityEngine;
using Zenject;

namespace SteampunkChess
{
    public class TestPOCO : IInitializable
    {
        public void Initialize()
        {
            Logger.Debug("Start");
        }
    }
}
