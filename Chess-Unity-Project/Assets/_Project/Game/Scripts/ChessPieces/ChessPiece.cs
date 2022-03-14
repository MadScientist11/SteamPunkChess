using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using DG.Tweening;
using SteamPunkChess;
using UnityEngine;

namespace SteampunkChess
{
    public abstract class ChessPiece
    {
        public Sequence MoveSequence { get; set; }

        public int CurrentX;
        public int CurrentY;
        public Team Team;
        public ChessPieceType ChessType { get; set; }
        public Transform PieceTransform { get; set; }


        protected IObjectTweener Tweener;

        public ChessPiece()
        {
            if (this is Knight)
                Tweener = new ArcTweener(1, .65f);
            else
                Tweener = new LineTweener(1);
        }


        public abstract List<Movement> GetAvailableMoves(PieceArrangement pieceArrangement, int tileCountX, int tileCountY);

        //public virtual ISpecialMove GetSpecialMove(ref ChessPiece[,] board, ref List<Vector2Int[]> moveList,
        //    ref List<Vector2Int> availableMoves)
        //{
        //    return new NoneSpecialMove();
        //}

        public Task PositionPiece(int x, int y, bool force = false)
        {
            CurrentX = x;
            CurrentY = y;
            Vector3 tile = TileSet.GetTileCenter(x, y);

            if (force)
            {
                PieceTransform.position = tile;
                return Task.CompletedTask;
            }

            return Tweener.MoveTo(PieceTransform, tile);
        }

        public bool IsFromSameTeam(ChessPiece piece)
        {
            return Team == piece.Team;
        }
    }
}