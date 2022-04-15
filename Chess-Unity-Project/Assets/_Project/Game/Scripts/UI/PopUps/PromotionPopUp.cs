using System;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using Photon.Realtime;
using SteampunkChess.NetworkService;
using SteampunkChess.PopUps;
using SteampunkChess.PopUpService;
using Zenject;

namespace SteampunkChess
{
    public class PromotionPopUp : PopUp
    {
        private IReadOnlyList<Movement> _moveList;
        private PieceArrangement _pieceArrangement;
        private IPopUpService _popUpService;
        private IGameFactory _gameFactory;
        private INetworkService _networkService;

        public override string PopUpKey { get; set; } = GameConstants.PopUps.PromotionWindow;

        [Inject]
        private void Construct(IPopUpService popUpService, INetworkService networkService)
        {
            _networkService = networkService;
            _popUpService = popUpService;
        }

        public override void Start()
        {
        }

        public override void Show(params object[] data)
        {
            _gameFactory = (IGameFactory) data[0];
            _moveList = (IReadOnlyList<Movement>) data[1];
            _pieceArrangement = (PieceArrangement) data[2];
            _networkService.PhotonRPCSender.OnChosePieceToPromote += RPC_ChoosePieceAndProcessPromotion;

            if (!_gameFactory.CachedGame.IsActivePlayer)
                _popUpService.HidePopUp(GameConstants.PopUps.PromotionWindow, HideType.Hide);
        }

        public void ChoosePieceAndProcessPromotion(int pieceIndex)
        {
            object[] content = {pieceIndex};
            _networkService.PhotonRPCSender.SendRPC(GameConstants.RPCMethodsByteCodes.OnChosePieceToPromote, content, ReceiverGroup.All, SendOptions.SendReliable);
        }

        private void RPC_ChoosePieceAndProcessPromotion(int pieceIndex)
        {
            if (pieceIndex < 2 || pieceIndex > 5)
                throw new ArgumentOutOfRangeException(nameof(pieceIndex), pieceIndex, "Cannot promote to piece with specified index");

            ChessPieceType pieceType = (ChessPieceType) pieceIndex;
            Movement lastMove = _moveList[_moveList.Count - 1];
            ChessPiece targetPawn = _pieceArrangement[lastMove.Destination.x, lastMove.Destination.y];
            Promotion();
            _popUpService.HidePopUp(GameConstants.PopUps.PromotionWindow, HideType.HideDestroyAndRelease);

            _gameFactory.CachedGame.WaitingForUserInput = false;

            void Promotion()
            {
                ChessPiece promQueen = _pieceArrangement.SpawnSinglePiece(pieceType, targetPawn.Team);
                targetPawn.Dispose();
                _pieceArrangement[lastMove.Destination.x, lastMove.Destination.y] = promQueen;
                promQueen.PositionPiece(lastMove.Destination.x, lastMove.Destination.y, true);
            }
        }

   
    }
}