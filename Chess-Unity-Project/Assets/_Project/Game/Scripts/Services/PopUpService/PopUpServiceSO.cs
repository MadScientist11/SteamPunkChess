using SteampunkChess.PopUps;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Zenject;

namespace SteampunkChess.PopUpService
{
    [CreateAssetMenu(fileName = "PopUpService", menuName = "Services/PopUpService")]
    public class PopUpServiceSO : ScriptableObject, IPopUpService
    {
        //TODO: stack of instances, grouping
        private readonly Dictionary<string, GameObject> _popUpsInstances = new Dictionary<string, GameObject>();

        private readonly Dictionary<string, AsyncOperationHandle<GameObject>> _asyncOperationHandles =
            new Dictionary<string, AsyncOperationHandle<GameObject>>();

        private DiContainer _container;

        [Inject]
        private void Construct(DiContainer container, ServiceContainer serviceContainer)
        {
            _container = container;
            serviceContainer.ServiceList.Add(this);
        }
        
        public string InitializationMessage => "PopUp initialization";

        public async Task Initialize()
        {
            await Task.Delay(2000);
        }


        public async void ShowPopUp(string popUpKey, params object[] data)
        {
            if (_popUpsInstances.ContainsKey(popUpKey))
            {
                _popUpsInstances[popUpKey].GetComponent<IPopUp>().Show(data);
                return;
            }

            if (_asyncOperationHandles.ContainsKey(popUpKey))
            {
                GameObject go = _container.InstantiatePrefab(_asyncOperationHandles[popUpKey].Result, FindObjectOfType<Canvas>().transform);
                go.GetComponent<IPopUp>().Show(data);
                _popUpsInstances[popUpKey] = go;
                return;
            }

            AsyncOperationHandle<GameObject> loadOp = Addressables.LoadAssetAsync<GameObject>(popUpKey);
            await loadOp.Task;
            if (loadOp.Status == AsyncOperationStatus.Succeeded)
            {
                _asyncOperationHandles.Add(popUpKey, loadOp);

                GameObject go = _container.InstantiatePrefab(loadOp.Result, FindObjectOfType<Canvas>().transform);
                go.GetComponent<IPopUp>().Show(data);
                _popUpsInstances.Add(popUpKey, go);
            }
            else
            {
                Debug.Log("Operation failed! Probably key doesn't exist.");
            }
        }

        public void HidePopUp(string popUpKey, HideType hideType)
        {
            if (!_popUpsInstances.ContainsKey(popUpKey)) return;
            IPopUp popUp = _popUpsInstances[popUpKey].GetComponent<IPopUp>();

            switch (hideType)
            {
                case HideType.Hide:
                    popUp.Hide();
                    break;
                case HideType.HideAndDestroy:
                    popUp.Hide(true);
                    _popUpsInstances.Remove(popUpKey);
                    break;
                case HideType.HideDestroyAndRelease:
                    popUp.OnDestroyed += () =>
                    {
                        Addressables.Release(_asyncOperationHandles[popUpKey]);
                        _asyncOperationHandles.Remove(popUpKey);
                    };
                    popUp.Hide(true);
                    _popUpsInstances.Remove(popUpKey);
                    break;
            }
        }

        public void HideAll(HideType hideType)
        {
            foreach (KeyValuePair<string, GameObject> instance in _popUpsInstances)
            {
                IPopUp popUp = _popUpsInstances[instance.Key].GetComponent<IPopUp>();

                switch (hideType)
                {
                    case HideType.Hide:
                        popUp.Hide();
                        break;
                    case HideType.HideAndDestroy:
                        popUp.Hide(true);
                        break;
                    case HideType.HideDestroyAndRelease:
                        popUp.OnDestroyed += () =>
                        {
                            Addressables.Release(_asyncOperationHandles[instance.Key]);
                            _asyncOperationHandles.Remove(instance.Key);
                        };
                        popUp.Hide(true);
                        break;
                }
            }

            _popUpsInstances.Clear();

        }
        
    }
}