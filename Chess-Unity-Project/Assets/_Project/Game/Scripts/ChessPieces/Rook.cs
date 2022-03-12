using System.Collections.Generic;
using SteampunkChess;
using UnityEngine;

namespace SteamPunkChess
{
    public class Rook : ChessPiece
    {

        
        public override List<Vector2Int> GetAvailableMoves(ref ChessPiece[,] board, int tileCountX, int tileCountY)
        {
            List<Vector2Int> r = new List<Vector2Int>();

            //Down
            for (int i = CurrentY - 1; i >= 0; i--)
            {
                if (board[CurrentX, i] == null)
                {
                    r.Add(new Vector2Int(CurrentX, i));
                }
                if (board[CurrentX, i] != null)
                {
                    if (!IsFromSameTeam(board[CurrentX, i]))
                        r.Add(new Vector2Int(CurrentX, i));


                    break;
                }
            }

            //Up
            for (int i = CurrentY + 1; i < tileCountY; i++)
            {
                if (board[CurrentX, i] == null)
                {
                    r.Add(new Vector2Int(CurrentX, i));
                }
                if (board[CurrentX, i] != null)
                {
                    if (!IsFromSameTeam(board[CurrentX, i]))
                        r.Add(new Vector2Int(CurrentX, i));


                    break;
                }
            }

            // left
            for (int i = CurrentX - 1; i >= 0; i--)
            {
                if (board[i, CurrentY] == null)
                {
                    r.Add(new Vector2Int(i, CurrentY));
                }
                if (board[i, CurrentY] != null)
                {
                    if (!IsFromSameTeam(board[i, CurrentY]))
                        r.Add(new Vector2Int(i, CurrentY));


                    break;
                }
            }
            // right
            for (int i = CurrentX + 1; i < tileCountX; i++)
            {
                if (board[i, CurrentY] == null)
                {
                    r.Add(new Vector2Int(i, CurrentY));
                }
                if (board[i, CurrentY] != null)
                {
                    if (!IsFromSameTeam(board[i, CurrentY]))
                        r.Add(new Vector2Int(i, CurrentY));


                    break;
                }
            }
            return r;
        }

    }
}