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

        private IInstantiator _instantiator;

        [Inject]
        private void Construct(IInstantiator instantiator, ServiceContainer serviceContainer)
        {
            _instantiator = instantiator;
            serviceContainer.ServiceList.Add(this);
        }

        public string InitializationMessage => "Initializing game services";

        public async Task Initialize()
        {
            await Task.Delay(2000);
        }


        public async void ShowPopUp(string popUpKey, params object[] data)
        {
            
            if (_popUpsInstances.ContainsKey(popUpKey))
            {
                IPopUp popUp = _popUpsInstances[popUpKey].GetComponent<IPopUp>();
                
                if (!popUp.IsVisible)
                    popUp.Show(data);
                
                return;
            }

            if (_asyncOperationHandles.ContainsKey(popUpKey))
            {
                GameObject go = _instantiator.InstantiatePrefab(_asyncOperationHandles[popUpKey].Result,
                    FindObjectOfType<Canvas>().transform);
                _popUpsInstances[popUpKey] = go;
                go.GetComponent<IPopUp>().Show(data);
                return;
            }

            AsyncOperationHandle<GameObject> loadOp = Addressables.LoadAssetAsync<GameObject>(popUpKey);
            await loadOp.Task;
            if (loadOp.Status == AsyncOperationStatus.Succeeded)
            {
                _asyncOperationHandles.Add(popUpKey, loadOp);

                GameObject go = _instantiator.InstantiatePrefab(loadOp.Result, FindObjectOfType<UICanvas>().transform);
                _popUpsInstances.Add(popUpKey, go);
                go.GetComponent<IPopUp>().Show(data);
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
                    break;
                case HideType.HideDestroyAndRelease:
                    popUp.OnDestroyed += () =>
                    {
                        Addressables.Release(_asyncOperationHandles[popUpKey]);
                        _asyncOperationHandles.Remove(popUpKey);
                    };
                    popUp.Hide(true);
                    break;
            }
        }

        public void RemoveInstanceFromInternalPool(string instanceKey)
        {
            if (_popUpsInstances.ContainsKey(instanceKey))
                _popUpsInstances.Remove(instanceKey);
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