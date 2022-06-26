using System.Collections;
using Cinemachine;
using SteampunkChess.NetworkService;
using UnityEngine;
using Zenject;


namespace SteampunkChess
{
    public class GameCameraController : MonoBehaviour
    {
        [SerializeField] private Transform _cameraPivot;
        [SerializeField] private CinemachineFreeLook _freeLookCamera;
        [SerializeField] private CinemachineVirtualCamera _topViewVirtualCamera;
        [SerializeField] private GameObject _inGameSideMenu;
        private Transform _chessBoard;
        private Team _playerTeam;
        private IInputSystem _inputSystem;
        private Camera _camera;
        private CinemachineBrain _cinemachineBrain;
        

        [Inject]
        private void Construct(IInputSystem inputSystem)
        {
            _inputSystem = inputSystem;
        }

        public void Initialize(Team playerTeam)
        {
            _playerTeam = playerTeam;
            
            _camera = GetComponent<Camera>();
            _chessBoard = FindObjectOfType<BoardInputHandler>().transform;
            _cinemachineBrain = GetComponent<CinemachineBrain>();
            SetPlayerStartingView();
         
            
            _inputSystem.OnCameraViewChanged += ChangeCameraView;

        }

        private void ChangeCameraView(KeyCode keyCode)
        {
            if (!_cinemachineBrain.IsBlending)
            {

                if (keyCode == KeyCode.Alpha1)
                {

                    _topViewVirtualCamera.Priority = 15;
                    _freeLookCamera.Priority = 10;
                    StartCoroutine(SetOrthographicAfterBlend(true));
                    _inGameSideMenu.SetActive(true);

                }
                else if (keyCode == KeyCode.Alpha2)
                {

                    _freeLookCamera.Priority = 15;
                    _topViewVirtualCamera.Priority = 10;
                    StartCoroutine(SetOrthographicAfterBlend(false));
                    _inGameSideMenu.SetActive(false);
                }
            }
        }

        IEnumerator SetOrthographicAfterBlend(bool value)
        {
            while (_cinemachineBrain.IsBlending)
            {
                yield return null;
            }

            _camera.orthographic = value;
            
        
        }
       
        private void SetPlayerStartingView()
        {
           if (_playerTeam == Team.White)
           {
               _cameraPivot.Rotate(Vector3.up, 180f);
               _freeLookCamera.m_XAxis.Value = 180f;
           }
           _freeLookCamera.Follow = _chessBoard;
           _freeLookCamera.LookAt = _chessBoard;
           _topViewVirtualCamera.Priority = 15;
           _freeLookCamera.Priority = 10;
            
        }
    }
}
