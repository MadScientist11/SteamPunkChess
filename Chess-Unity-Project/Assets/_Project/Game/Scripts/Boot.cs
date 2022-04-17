using System;
using System.Threading.Tasks;
using SteampunkChess.LocalizationSystem;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Zenject;

namespace SteampunkChess
{
    public class Boot : MonoBehaviour
    {
        [SerializeField] private AssetReference _signInScene;
        [SerializeField] private TextMeshProUGUI _loadingText;
        
        private ServiceContainer _serviceContainer;
        private bool _isInitialized;


        [Inject]
        private void Construct(ServiceContainer serviceContainer)
        {
            _serviceContainer = serviceContainer;
        }

        private void Start()
        {
            ScreenResolution screenResolution = (ScreenResolution) Prefs.Settings.WindowSize;

            (int width, int height, bool fullScreen) resolutionInfo = screenResolution switch
            {
                ScreenResolution.XGA => (1024, 768, false),
                ScreenResolution.HD => (1366, 768, false),
                ScreenResolution.FullHD => (1920, 1080, true),
                ScreenResolution.QHD => (2560, 1440, true),
                _ => throw new ArgumentOutOfRangeException()
            };
            Screen.SetResolution(resolutionInfo.width, resolutionInfo.height, resolutionInfo.fullScreen);
        }

        private async Task InitializeServices()
        {
            foreach (var service in _serviceContainer.ServiceList)
            {
                _loadingText.text = service.InitializationMessage;
                Logger.Debug(service.InitializationMessage);
                await service.Initialize();
            }
        }

        private async void Update()
        {
            if (Input.anyKeyDown && !_isInitialized)
            {
                _loadingText.gameObject.SetActive(true);
                _isInitialized = true;
                await InitializeServices();
                _signInScene.LoadSceneAsync();
                ApplySettings();
            }
        }

        private void ApplySettings()
        {
            QualitySettings.vSyncCount = Convert.ToInt32(Prefs.Settings.Vsync);
            QualitySettings.masterTextureLimit = Prefs.Settings.TextureQuality;
        }
        
     
    }


}