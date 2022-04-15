using UnityEngine;

namespace SteampunkChess
{
    public class GameCameraController : MonoBehaviour
    {
        [SerializeField] private Transform _cameraPivot;
        private Team _playerTeam;

        public void Initialize(Team playerTeam)
        {
            _playerTeam = playerTeam;
            SetPlayerStartingView();
        }
       
        private void SetPlayerStartingView()
        {
            if(_playerTeam == Team.White)
                _cameraPivot.Rotate(Vector3.up, 180f);
        }
    }
}
