using System.Collections.Generic;
using SteampunkChess;
using UnityEngine;

namespace SteamPunkChess
{
    public class Bishop : ChessPiece
    {
      
        
        public override List<Movement> GetAvailableMoves(PieceArrangement pieceArrangement, int tileCountX, int tileCountY, List<Movement> moveHistory, List<Movement> availableMoves)
        {
            List<Movement> r = new List<Movement>();
            
            //Top right 
            for (int x = CurrentX + 1, y = CurrentY + 1; x < tileCountX && y < tileCountY; x++, y++)
            {
                if (pieceArrangement[x, y] == null)
                {
                    r.Add(new Movement(new Vector2Int(CurrentX, CurrentY), new Vector2Int(x, y), pieceArrangement));
                }
                else
                {
                    if (!IsFromSameTeam(pieceArrangement[x, y]))
                        r.Add(new Movement(new Vector2Int(CurrentX, CurrentY), new Vector2Int(x, y), pieceArrangement));

                    break;
                }
            }
            
            //Top left 
            for (int x = CurrentX - 1, y = CurrentY + 1; x >= 0 && y < tileCountY; x--, y++)
            {
                if (pieceArrangement[x, y] == null)
                {
                    r.Add(new Movement(new Vector2Int(CurrentX, CurrentY), new Vector2Int(x, y), pieceArrangement));
                }
                else
                {
                    if (!IsFromSameTeam(pieceArrangement[x, y]))
                        r.Add(new Movement(new Vector2Int(CurrentX, CurrentY), new Vector2Int(x, y), pieceArrangement));

                    break;
                }
            }
            //Bottom right 
            for (int x = CurrentX + 1, y = CurrentY - 1; x < tileCountX && y >= 0; x++, y--)
            {
                if (pieceArrangement[x, y] == null)
                {
                    r.Add(new Movement(new Vector2Int(CurrentX, CurrentY), new Vector2Int(x, y), pieceArrangement));
                }
                else
                {
                    if (!IsFromSameTeam(pieceArrangement[x, y]))
                        r.Add(new Movement(new Vector2Int(CurrentX, CurrentY), new Vector2Int(x, y), pieceArrangement));

                    break;
                }
            }

            //Bottom left 
            for (int x = CurrentX - 1, y = CurrentY - 1; x >= 0 && y >= 0; x--, y--)
            {
                if (pieceArrangement[x, y] == null)
                {
                    r.Add(new Movement(new Vector2Int(CurrentX, CurrentY), new Vector2Int(x, y), pieceArrangement));
                }
                else
                {
                    if (!IsFromSameTeam(pieceArrangement[x, y]))
                        r.Add(new Movement(new Vector2Int(CurrentX, CurrentY), new Vector2Int(x, y), pieceArrangement));

                    break;
                }
            }
            return r;
        }
    }
}