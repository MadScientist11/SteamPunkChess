using UnityEngine;
using Zenject;

namespace SteampunkChess
{
    public class MenuEntryPoint : MonoBehaviour
    {
        private IAudioSystem _audioSystem;

        [Inject]
        private void Construct(IAudioSystem audioSystem)
        {
            _audioSystem = audioSystem;
        }

        private void Awake()
        {
            _audioSystem.StartBackgroundMusicLoop();
        }
        
       
       
    }
}