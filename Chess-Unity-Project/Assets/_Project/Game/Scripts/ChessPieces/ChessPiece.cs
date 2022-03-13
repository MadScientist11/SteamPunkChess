using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
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


        //private IObjectTweener tweener;
     
        

        public abstract List<Vector2Int> GetAvailableMoves(ref ChessPiece[,] board, int tileCountX, int tileCountY);

        //public virtual ISpecialMove GetSpecialMove(ref ChessPiece[,] board, ref List<Vector2Int[]> moveList,
        //    ref List<Vector2Int> availableMoves)
        //{
        //    return new NoneSpecialMove();
        //}

        private void SetPosition(Vector3 position, bool force = false)
        {
            if (force)
            {
                PieceTransform.position = position;
                return;
            }

           // MoveSequence = tweener.MoveTo(transform, position);
        }
        public void PositionPiece(int x, int y, bool force = false)
        {
            CurrentX = x;
            CurrentY = y;
            Vector3 tileCenter = TileSet.GetTileCenter(x, y);
            SetPosition(tileCenter, true);
        }

        public bool IsFromSameTeam(ChessPiece piece)
        {
            return Team == piece.Team;
        }
    }
}
