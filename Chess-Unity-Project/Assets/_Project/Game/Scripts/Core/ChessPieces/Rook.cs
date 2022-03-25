using System.Collections.Generic;
using SteampunkChess;
using UnityEngine;

namespace SteampunkChess
{
    public class Rook : ChessPiece
    {
        public override List<Movement> GetAvailableMoves(PieceArrangement pieceArrangement, int tileCountX, int tileCountY, List<Movement> moveHistory)
        {
            List<Movement> r = new List<Movement>();

            //Down
            for (int i = CurrentY - 1; i >= 0; i--)
            {
                if (pieceArrangement[CurrentX, i] == null)
                {
                    r.Add(new Movement(new Vector2Int(CurrentX, CurrentY), new Vector2Int(CurrentX, i), pieceArrangement));
                }
                if (pieceArrangement[CurrentX, i] != null)
                {
                    if (!IsFromSameTeam(pieceArrangement[CurrentX, i]))
                        r.Add(new Movement(new Vector2Int(CurrentX, CurrentY), new Vector2Int(CurrentX, i), pieceArrangement));


                    break;
                }
            }

            //Up
            for (int i = CurrentY + 1; i < tileCountY; i++)
            {
                if (pieceArrangement[CurrentX, i] == null)
                {
                    r.Add(new Movement(new Vector2Int(CurrentX, CurrentY), new Vector2Int(CurrentX, i), pieceArrangement));
                }
                if (pieceArrangement[CurrentX, i] != null)
                {
                    if (!IsFromSameTeam(pieceArrangement[CurrentX, i]))
                        r.Add(new Movement(new Vector2Int(CurrentX, CurrentY), new Vector2Int(CurrentX, i), pieceArrangement));


                    break;
                }
            }

            // left
            for (int i = CurrentX - 1; i >= 0; i--)
            {
                if (pieceArrangement[i, CurrentY] == null)
                {
                    r.Add(new Movement(new Vector2Int(CurrentX, CurrentY), new Vector2Int(i, CurrentY), pieceArrangement));
                }
                if (pieceArrangement[i, CurrentY] != null)
                {
                    if (!IsFromSameTeam(pieceArrangement[i, CurrentY]))
                        r.Add(new Movement(new Vector2Int(CurrentX, CurrentY), new Vector2Int(i, CurrentY), pieceArrangement));


                    break;
                }
            }
            // right
            for (int i = CurrentX + 1; i < tileCountX; i++)
            {
                if (pieceArrangement[i, CurrentY] == null)
                {
                    r.Add(new Movement(new Vector2Int(CurrentX, CurrentY), new Vector2Int(i, CurrentY), pieceArrangement));
                }
                if (pieceArrangement[i, CurrentY] != null)
                {
                    if (!IsFromSameTeam(pieceArrangement[i, CurrentY]))
                        r.Add(new Movement(new Vector2Int(CurrentX, CurrentY), new Vector2Int(i, CurrentY), pieceArrangement));
                    
                    break;
                }
            }
            return r;
        }

    }
}