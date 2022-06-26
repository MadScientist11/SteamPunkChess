using System;
using SteampunkChess.CloudService;
using SteampunkChess.LocalizationSystem;
using SteampunkChess.PopUps;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;
using Zenject;

namespace SteampunkChess
{
    public enum ScreenResolution
    {
        XGA,
        HD,
        FullHD,
        QHD
    }

    public class SettingsPopUp : PopUp
    {
        [Header("SettingsPopUp")]
        [SerializeField] private TMP_Dropdown _languageDropdown;
        [SerializeField] private TMP_Dropdown _textureQualityDropdown;
        [SerializeField] private TMP_Dropdown _windowResolutionDropdown;

        [SerializeField] private Toggle _musicToggle;
        [SerializeField] private Toggle _soundsToggle;
        [SerializeField] private Toggle _vsyncToggle;
        [SerializeField] private Toggle _postProcessingToggle
            ;
        
        private IAudioSystem _audioSystem;
        private ILocalizationSystem _localizationSystem;
        private ICloudService _cloudService;
        private IInputSystem _inputSystem;
        public override string PopUpKey { get; set; } = GameConstants.PopUps.SettingsPopUp;

        [Inject]
        private void Construct(IAudioSystem audioSystem, ILocalizationSystem localizationSystem, ICloudService cloudService, IInputSystem inputSystem)
        {
            _inputSystem = inputSystem;
            _cloudService = cloudService;
            _localizationSystem = localizationSystem;
            _audioSystem = audioSystem;
            
        }

        private void Awake()
        {
            int index = Prefs.Settings.Language switch
            {
                "uk" => 0,
                "ru" => 1,
                "en" => 2,
                _ => throw new ArgumentOutOfRangeException()
            };
            _languageDropdown.SetValueWithoutNotify(index);
            _textureQualityDropdown.SetValueWithoutNotify(Prefs.Settings.TextureQuality);
            _windowResolutionDropdown.SetValueWithoutNotify(Prefs.Settings.WindowSize); 
            _musicToggle.SetIsOnWithoutNotify(Prefs.Settings.Music);
            _soundsToggle.SetIsOnWithoutNotify(Prefs.Settings.Sounds);
            _vsyncToggle.SetIsOnWithoutNotify(Prefs.Settings.Vsync);
            _postProcessingToggle.SetIsOnWithoutNotify(Prefs.Settings.PostProcessing);
        }

        #region Video

        public void ChangeResolutionSize(int index)
        {
            ScreenResolution screenResolution = (ScreenResolution) _windowResolutionDropdown.value;

            (int width, int height, bool fullScreen) resolutionInfo = screenResolution switch
            {
                ScreenResolution.XGA => (1024, 768, false),
                ScreenResolution.HD => (1366, 768, false),
                ScreenResolution.FullHD => (1920, 1080, true),
                ScreenResolution.QHD => (2560, 1440, true),
                _ => throw new ArgumentOutOfRangeException()
            };
            Prefs.Settings.WindowSize = _windowResolutionDropdown.value;
            Screen.SetResolution(resolutionInfo.width, resolutionInfo.height, resolutionInfo.fullScreen);
        }

        public void ToggleVSync(bool value)
        {
            Prefs.Settings.Vsync = value;
            QualitySettings.vSyncCount = Convert.ToInt32(value);
        }

        public void TogglePostProcessing(bool value)
        {
            Prefs.Settings.PostProcessing = value;
        }

        public void ChangeTextureQuality(int index)
        {
            Prefs.Settings.TextureQuality = _textureQualityDropdown.value;
            QualitySettings.masterTextureLimit = Prefs.Settings.TextureQuality;
        }

        #endregion

        #region Audio

        public void ToggleMusic(bool value)
        {
            Prefs.Settings.Music = value;
            _audioSystem.StartBackgroundMusicLoop();
            _audioSystem.SetMusicVolume();
        }

        public void ToggleSounds(bool value)
        {
            Prefs.Settings.Sounds = value;
            _audioSystem.SetSoundsVolume();
        }

        #endregion

        #region Game

        public void ChangeLanguage(int index)
        {
            string langIdentifier = _languageDropdown.value switch
            {
                0 => "uk",
                1 => "ru",
                2 => "en",
                _ => throw new ArgumentOutOfRangeException(nameof(index), index, null)
            };
            _localizationSystem.ChangeLanguage(langIdentifier);
        }

        public void Logout()
        {
            _cloudService.Logout();
            Prefs.RememberMe = false;
            _inputSystem.OnBackButtonPressed = null;
            Addressables.LoadSceneAsync("SignInScene");
            
        }

        #endregion
    }
}