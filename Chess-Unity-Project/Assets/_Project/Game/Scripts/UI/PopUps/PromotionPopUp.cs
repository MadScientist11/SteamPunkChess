using System;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using SteampunkChess.PopUps;
using SteampunkChess.PopUpService;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace SteampunkChess
{
    public class PromotionPopUp : PopUp, IOnEventCallback
    {
        private IReadOnlyList<Movement> _moveList;
        private PieceArrangement _pieceArrangement;
        private IPopUpService _popUpService;
        private IGameFactory _gameFactory;
        private const int SetPromotionPieceEvent = 1;

        [Inject]
        private void Construct(IPopUpService popUpService)
        {
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

            if (!_gameFactory.CachedGame.IsActivePlayer)
                _popUpService.HidePopUp(GameConstants.PopUps.PromotionWindow, HideType.Hide);
        }

        private void ChoosePieceAndProcessPromotion(int pieceIndex)
        {
            if (pieceIndex < 2 || pieceIndex > 5)
                throw new ArgumentOutOfRangeException(nameof(pieceIndex), pieceIndex, "Cannot promote to piece with specified index");

            ChessPieceType pieceType = (ChessPieceType) pieceIndex;
            Movement lastMove = _moveList[_moveList.Count - 1];
            ChessPiece targetPawn = _pieceArrangement[lastMove.Destination.x, lastMove.Destination.y];
            
            Promotion();

            _popUpService.HidePopUp(GameConstants.PopUps.PromotionWindow, HideType.HideDestroyAndRelease);

            void Promotion()
            {
                ChessPiece promQueen = _pieceArrangement.SpawnSinglePiece(pieceType, targetPawn.Team);
                targetPawn.Dispose();
                _pieceArrangement[lastMove.Destination.x, lastMove.Destination.y] = promQueen;
                promQueen.PositionPiece(lastMove.Destination.x, lastMove.Destination.y, true);
            }
        }

        private void OnEnable()
        {
            PhotonNetwork.AddCallbackTarget(this);
        }

        private void OnDisable()
        {
            PhotonNetwork.RemoveCallbackTarget(this);
        }

        public void OnEvent(EventData photonEvent)
        {
            byte eventCode = photonEvent.Code;

            if (eventCode == SetPromotionPieceEvent)
            {
                object[] data = (object[]) photonEvent.CustomData;
                ChoosePieceAndProcessPromotion((int) data[0]);
                _gameFactory.CachedGame.WaitingForUserInput = false;
            }
        }


        public void RPC_ProcessPromotion(int pieceIndex)
        {
            int pieceNumber = pieceIndex;
            object[] content = {pieceNumber};
            RaiseEventOptions raiseEventOptions = new RaiseEventOptions {Receivers = ReceiverGroup.All};
            PhotonNetwork.RaiseEvent(SetPromotionPieceEvent, content, raiseEventOptions, SendOptions.SendReliable);
        }
    }
}