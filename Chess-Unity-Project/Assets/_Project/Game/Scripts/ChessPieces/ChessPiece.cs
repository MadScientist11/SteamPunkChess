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

        public void SetPosition(Vector3 position, bool force = false)
        {
            if (force)
            {
                PieceTransform.position = position;
                return;
            }

           // MoveSequence = tweener.MoveTo(transform, position);
        }

        public bool IsFromSameTeam(ChessPiece piece)
        {
            return Team == piece.Team;
        }
    }
}
