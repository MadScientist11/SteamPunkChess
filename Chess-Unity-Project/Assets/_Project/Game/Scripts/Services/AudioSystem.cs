using System;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Audio;
using Zenject;

namespace SteampunkChess
{
    public enum Sounds
    {
        IncorrectPasswordSound,
        LoseSound,
        WinSound,
        PieceMoveSound
    }

    public interface IAudioSystem : IService
    {
        void StartBackgroundMusicLoop();
        void PlaySound(Sounds sounds);

        void SetMusicVolume();

        void SetSoundsVolume();
    }

    [CreateAssetMenu(fileName = "AudioSystem", menuName = "Services/AudioSystem")]
    public class AudioSystem : ScriptableObject, IAudioSystem
    {
        public string InitializationMessage { get; } = "Initializing game services";

        private AudioSource _musicAudioSource;
        private AudioSource _soundsAudioSource;

        [SerializeField] private AudioMixer _audioMixer;

        [Header("Music")] 
        [SerializeField] private AudioClip[] _backgroundMusic;

        [Header("Sounds")] 
        [SerializeField] private AudioClip _incorrectPasswordSound;
        [SerializeField] private AudioClip _loseSound;
        [SerializeField] private AudioClip _winSound;
        [SerializeField] private AudioClip _pieceMoveSound;

        [Inject]
        private void Construct(ServiceContainer serviceContainer)
        {
            serviceContainer.ServiceList.Add(this);
        }

        public async Task Initialize()
        {
            _musicAudioSource = new GameObject("MusicAudioSource").AddComponent<AudioSource>();
            _soundsAudioSource = new GameObject("SoundsAudioSource").AddComponent<AudioSource>();

            _musicAudioSource.outputAudioMixerGroup = _audioMixer.FindMatchingGroups("Master/Music")[0];
            _soundsAudioSource.outputAudioMixerGroup = _audioMixer.FindMatchingGroups("Master/Sounds")[0];

            DontDestroyOnLoad(_musicAudioSource);
            DontDestroyOnLoad(_soundsAudioSource);
            await Task.CompletedTask;
        }

        public void StartBackgroundMusicLoop()
        {
            if (!_musicAudioSource.isPlaying && Prefs.Settings.Music)
                CoroutineStarter.Instance.StartCoroutine(PlayMusic());
        }


        public void SetMusicVolume()
        {
            int newValue = Prefs.Settings.Music ? 0 : -80;
            _audioMixer.SetFloat("MusicVolume", newValue);
        }

        public void SetSoundsVolume()
        {
            int newValue = Prefs.Settings.Music ? 0 : -80;
            _audioMixer.SetFloat("SoundsVolume", newValue);
        }

        public void PlaySound(Sounds sounds)
        {
            var soundToPlay = sounds switch
            {
                Sounds.IncorrectPasswordSound => _incorrectPasswordSound,
                Sounds.LoseSound => _loseSound,
                Sounds.WinSound => _winSound,
                Sounds.PieceMoveSound => _pieceMoveSound,
                _ => throw new ArgumentOutOfRangeException(nameof(sounds), sounds, null)
            };
            _soundsAudioSource.PlayOneShot(soundToPlay);
        }

        private IEnumerator PlayMusic()
        {
            for (int i = 0; i < _backgroundMusic.Length; i++)
            {
                _musicAudioSource.PlayOneShot(_backgroundMusic[i]);
                while (_musicAudioSource.isPlaying)
                    yield return null;
            }

            CoroutineStarter.Instance.StartCoroutine(PlayMusic());
        }
    }
}