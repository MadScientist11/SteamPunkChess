using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Object = UnityEngine.Object;

namespace SteampunkChess
{
    public abstract class ChessPiece : IDisposable
    {
        public int CurrentX;
        public int CurrentY;
        public Team Team;
        public ChessPieceType ChessType { get; set; }
        public Transform PieceTransform { get; set; }


        protected IObjectTweener Tweener;

        public static event Action<ChessPiece> OnPieceDestroyed;
        public static event Action<ChessPiece> OnPieceSpawned;
        

        public void Initialize()
        {
            Tweener = this switch
            {
                Knight _ => new ArcTweener(1, .65f),
                _ => new LineTweener(1)
            };
            OnPieceSpawned?.Invoke(this);
        }


        public abstract List<Movement> GetAvailableMoves(PieceArrangement pieceArrangement, int tileCountX, int tileCountY, List<Movement> moveHistory);

        public ChessPiece ShallowCopy()
        {
            return (ChessPiece) this.MemberwiseClone();
        }
   

        public Task PositionPiece(int x, int y, bool force = false, IObjectTweener tweener = null)
        {
            CurrentX = x;
            CurrentY = y;
            Vector3 tile = TileSet.GetTileCenter(x, y);

            if (force)
            {
                PieceTransform.position = tile;
                return Task.CompletedTask;
            }

            return tweener == null ? Tweener.MoveTo(PieceTransform, tile) : tweener.MoveTo(PieceTransform,tile);
        }

        public bool IsFromSameTeam(ChessPiece piece)
            => Team == piece.Team;

        public void Dispose()
        {
            Object.Destroy(PieceTransform.gameObject);
            OnPieceDestroyed?.Invoke(this);
        }
    }
}