using System;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Zenject;



namespace SteampunkChess
{
    public class Boot : MonoBehaviour
    {
        [SerializeField] private AssetReference _mainMenuScene;
        [SerializeField] private TextMeshProUGUI _loadingText;
        private ServiceContainer _serviceContainer;
        private bool _isInitialized;
       
        [Inject]
        private void Construct(ServiceContainer serviceContainer)
        {
            _serviceContainer = serviceContainer;
            
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
            if (Input.GetKeyDown(KeyCode.Space) && !_isInitialized)
            {
                _isInitialized = true;
                await InitializeServices();
                _mainMenuScene.LoadSceneAsync();
            }
        }
    }


}