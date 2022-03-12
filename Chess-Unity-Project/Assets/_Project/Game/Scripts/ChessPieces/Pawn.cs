using System.Collections.Generic;
using SteampunkChess;
using UnityEngine;

namespace SteamPunkChess
{
    public class Pawn : ChessPiece
    { 

        public override List<Vector2Int> GetAvailableMoves(ref ChessPiece[,] board, int tileCountX, int tileCountY)
        {
            //one in front
            List<Vector2Int> r = new List<Vector2Int>();
            int direction = Team == 0 ? 1 : -1;
            if (CurrentY + direction < tileCountY)
            {
                if (board[CurrentX, CurrentY + direction] == null)
                    r.Add(new Vector2Int(CurrentX, CurrentY + direction));

                // two in front
                if (board[CurrentX, CurrentY + direction] == null)
                {
                    if (Team == Team.White && CurrentY == 1 && board[CurrentX, CurrentY + direction * 2] == null)
                        r.Add(new Vector2Int(CurrentX, CurrentY + direction * 2));

                    if (Team == Team.Black && CurrentY == 6 && board[CurrentX, CurrentY + direction * 2] == null)
                        r.Add(new Vector2Int(CurrentX, CurrentY + direction * 2));

                }


                //Diagonal move
                if (CurrentX != tileCountX - 1)
                    if (board[CurrentX + 1, CurrentY + direction] != null && !IsFromSameTeam(board[CurrentX + 1, CurrentY + direction]))
                        r.Add(new Vector2Int(CurrentX + 1, CurrentY + direction));
                if (CurrentX != 0)
                    if (board[CurrentX - 1, CurrentY + direction] != null && !IsFromSameTeam(board[CurrentX - 1, CurrentY + direction]))
                        r.Add(new Vector2Int(CurrentX - 1, CurrentY + direction));
            }
            return r;
        }
        
        /*public override ISpecialMove GetSpecialMove(ref ChessPiece[,] board, ref List<Vector2Int[]> moveList, ref List<Vector2Int> availableMoves)
        {
            int direction = team == 0 ? 1 : -1;
            if (team == Team.White && currentY == 6 || team == Team.Black && currentY == 1)
                return new Promotion();


            if (moveList.Count > 0)
            {
                var lastMove = moveList[moveList.Count - 1];
                if (board[lastMove[1].x, lastMove[1].y].chessType == ChessPieceType.Pawn)
                {
                    if (Mathf.Abs(lastMove[1].y - lastMove[0].y) == 2)
                    {
                        if (!IsFromSameTeam(board[lastMove[1].x, lastMove[1].y]))
                        {
                            if (lastMove[1].y == currentY)
                            {
                                if (lastMove[1].x == currentX - 1)
                                {
                                    availableMoves.Add(new Vector2Int(currentX - 1, currentY + direction));
                                    return new EnPassant();
                                }
                                if (lastMove[1].x == currentX + 1)
                                {
                                    availableMoves.Add(new Vector2Int(currentX + 1, currentY + direction));
                                    return new EnPassant();
                                }
                            }
                        }
                    }
                }
            }
            return new NoneSpecialMove();
        }*/

       
    }
}