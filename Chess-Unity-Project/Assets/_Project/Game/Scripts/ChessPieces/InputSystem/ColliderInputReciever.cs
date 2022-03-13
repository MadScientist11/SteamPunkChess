using UnityEngine;

namespace SteamPunkChess
{
    public class ColliderInputReciever : MonoBehaviour
    {
        private const string TileLayerMask = "Tile";
        private Camera _camera;

        private IInputHandler<GameObject> _inputHandler;
        private void Start()
        {
            _inputHandler = GetComponent<IInputHandler<GameObject>>();
            _camera = Camera.main;
        }
        private void Update()
        {
            Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit info, 100, LayerMask.GetMask(TileLayerMask)))
            {
                _inputHandler.ProcessInput(info.transform.gameObject);
            }
        }
    }
}