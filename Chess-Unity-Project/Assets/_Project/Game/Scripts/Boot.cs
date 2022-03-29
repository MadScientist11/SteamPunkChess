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
        [SerializeField] private GameObject _loadingGO;
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
            if (Input.anyKeyDown && !_isInitialized)
            {
                _loadingGO.SetActive(true);
                _loadingText.gameObject.SetActive(true);
                _isInitialized = true;
                await InitializeServices();
                _mainMenuScene.LoadSceneAsync();
            }
        }
    }


}