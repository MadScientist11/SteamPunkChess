using ExitGames.Client.Photon;
using Photon.Realtime;
using SteampunkChess.NetworkService;
using UnityEngine;

namespace SteampunkChess
{
    public class MultiplayerBoard : ChessBoard
    {
        private readonly INetworkService _networkService;

        public MultiplayerBoard(ChessBoardData chessBoardData, MoveListingData moveListingData, INetworkService networkService) : base(chessBoardData, moveListingData)
        {
            _networkService = networkService;
            _networkService.PhotonRPCSender.OnMoveToEvent += RPC_MoveTo;
            _networkService.PhotonRPCSender.OnSelectAndShowAvailableMovesEvent += RPC_SelectPieceAndShowAvailableMoves;
        }

        protected override void MoveTo(Vector2 coords)
        {
            object[] content = {coords};
            _networkService.PhotonRPCSender.SendRPC(GameConstants.RPCMethodsByteCodes.OnMoveToCode, content, ReceiverGroup.All, SendOptions.SendReliable);
        }

        protected override void SelectPieceAndShowAvailableMoves(Vector2 coords)
        {
            object[] content = {coords};
            _networkService.PhotonRPCSender.SendRPC(GameConstants.RPCMethodsByteCodes.OnSelectAndShowAvailableMovesCode, content, ReceiverGroup.All, SendOptions.SendReliable);
        }
        
        private void RPC_SelectPieceAndShowAvailableMoves(Vector2 coords)
        {
            Vector2Int intCoords = new Vector2Int(Mathf.RoundToInt(coords.x), Mathf.RoundToInt(coords.y));
            OnSelectPieceAndShowAvailableMoves(intCoords);
        }
        
        private void RPC_MoveTo(Vector2 coords)
        {
            Vector2Int intCoords = new Vector2Int(Mathf.RoundToInt(coords.x), Mathf.RoundToInt(coords.y));
            OnMoveTo(intCoords);
        }

       
    }
}