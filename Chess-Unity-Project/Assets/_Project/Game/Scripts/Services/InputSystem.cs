using System;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using Zenject;


namespace SteampunkChess
{
    public interface IInputSystem : IService
    {
        public bool CheckForInput { get; }

        public event Action OnBackButtonPressed;
    }
    
    [CreateAssetMenu(fileName = "InputSystem", menuName = "Services/InputSystem")]
    public class InputSystem : ScriptableObject, IInputSystem
    {
        public bool CheckForInput { get; } = true;
        
        public event Action OnBackButtonPressed;
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
                yield return null;
            }
        }

        
    }
}
