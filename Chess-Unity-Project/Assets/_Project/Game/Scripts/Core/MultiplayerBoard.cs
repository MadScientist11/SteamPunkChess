using Photon.Pun;
using SteampunkChess.NetworkService;
using UnityEngine;

namespace SteampunkChess
{
    public class MultiplayerBoard : ChessBoard
    {
        private readonly INetworkService _networkService;

        public MultiplayerBoard(GameDataSO gameDataSO, INetworkService networkService) : base(gameDataSO)
        {
            _networkService = networkService;
        }

        protected override void MoveTo(Vector2 coords)
        {
            _networkService.SendRPC(nameof(RPC_MoveTo), RpcTarget.All, coords);
        }

        protected override void SelectPieceAndShowAvailableMoves(Vector2 coords)
        {
            _networkService.SendRPC(nameof(RPC_SelectPieceAndShowAvailableMoves), RpcTarget.All, coords);
        }


        [PunRPC]
        private void RPC_SelectPieceAndShowAvailableMoves(Vector2 coords)
        {
            Vector2Int intCoords = new Vector2Int(Mathf.RoundToInt(coords.x), Mathf.RoundToInt(coords.y));
            OnSelectPieceAndShowAvailableMoves(intCoords);
        }

        [PunRPC]
        private void RPC_MoveTo(Vector2 coords)
        {
            Vector2Int intCoords = new Vector2Int(Mathf.RoundToInt(coords.x), Mathf.RoundToInt(coords.y));
            OnMoveTo(intCoords);
        }

       
    }
}