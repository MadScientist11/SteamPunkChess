using System;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using Zenject;


namespace SteampunkChess
{
    [CreateAssetMenu(fileName = "InputSystem", menuName = "Services/InputSystem")]
    public class InputSystem : ScriptableObject, IInputSystem
    {
        public bool CheckForInput { get; } = true;

        public Action OnBackButtonPressed { get; set; }
        
        public Action<KeyCode> OnCameraViewChanged { get; set; }
        public string InitializationMessage { get; } = "Initializing game services";
        
        [Inject]
        private void Construct(ServiceContainer serviceContainer)
        {
            serviceContainer.ServiceList.Add(this);
        }
        
        public async Task Initialize()
        {
            CoroutineStarter.Instance.StartCoroutine(CheckForInputUpdate());
            await Task.CompletedTask;
        }


        private IEnumerator CheckForInputUpdate()
        {
            while (CheckForInput)
            {
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    OnBackButtonPressed?.Invoke();
                    OnBackButtonPressed = null;
                }

                if (Input.GetKeyDown(KeyCode.Alpha1))
                {
                    OnCameraViewChanged?.Invoke(KeyCode.Alpha1);
                }
                
                if(Input.GetKeyDown(KeyCode.Alpha2))
                {
                    OnCameraViewChanged?.Invoke(KeyCode.Alpha2);
                }
                yield return null;
            }
        }

        
    }
}
