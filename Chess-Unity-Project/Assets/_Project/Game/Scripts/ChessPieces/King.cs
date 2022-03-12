using System.Collections.Generic;
using SteampunkChess;
using UnityEngine;


namespace SteamPunkChess
{
    public class King : ChessPiece
    {
        //[SerializeField] private SOGameFenDataSO gameFenDataSO;
        

        public override List<Vector2Int> GetAvailableMoves(ref ChessPiece[,] board, int tileCountX, int tileCountY)
        {
            List<Vector2Int> r = new List<Vector2Int>();

            // Right
            if (CurrentX + 1 < tileCountX)
            {
                if (board[CurrentX + 1, CurrentY] == null)
                    r.Add(new Vector2Int(CurrentX + 1, CurrentY));
                else if (!IsFromSameTeam(board[CurrentX + 1, CurrentY]))
                    r.Add(new Vector2Int(CurrentX + 1, CurrentY));

                // Top right
                if (CurrentY + 1 < tileCountY)
                    if (board[CurrentX + 1, CurrentY + 1] == null)
                        r.Add(new Vector2Int(CurrentX + 1, CurrentY + 1));
                    else if (!IsFromSameTeam(board[CurrentX + 1, CurrentY + 1]))
                        r.Add(new Vector2Int(CurrentX + 1, CurrentY + 1));

                // Bottom right
                if (CurrentY - 1 >= 0)
                    if (board[CurrentX + 1, CurrentY - 1] == null)
                        r.Add(new Vector2Int(CurrentX + 1, CurrentY - 1));
                    else if (!IsFromSameTeam(board[CurrentX + 1, CurrentY - 1]))
                        r.Add(new Vector2Int(CurrentX + 1, CurrentY - 1));
            }
            // Left
            if (CurrentX - 1 >= 0)
            {
                if (board[CurrentX - 1, CurrentY] == null)
                    r.Add(new Vector2Int(CurrentX - 1, CurrentY));
                else if (!IsFromSameTeam(board[CurrentX - 1, CurrentY]))
                    r.Add(new Vector2Int(CurrentX - 1, CurrentY));

                // Top right
                if (CurrentY + 1 < tileCountY)
                    if (board[CurrentX - 1, CurrentY + 1] == null)
                        r.Add(new Vector2Int(CurrentX - 1, CurrentY + 1));
                    else if (!IsFromSameTeam(board[CurrentX - 1, CurrentY + 1]))
                        r.Add(new Vector2Int(CurrentX - 1, CurrentY + 1));

                // Bottom right
                if (CurrentY - 1 >= 0)
                    if (board[CurrentX - 1, CurrentY - 1] == null)
                        r.Add(new Vector2Int(CurrentX - 1, CurrentY - 1));
                    else if (!IsFromSameTeam(board[CurrentX - 1, CurrentY - 1]))
                        r.Add(new Vector2Int(CurrentX - 1, CurrentY - 1));
            }
            //Up
            if (CurrentY + 1 < tileCountY)
                if (board[CurrentX, CurrentY + 1] == null || !IsFromSameTeam(board[CurrentX, CurrentY + 1]))
                    r.Add(new Vector2Int(CurrentX, CurrentY + 1));
            //Down
            if (CurrentY - 1 >= 0)
                if (board[CurrentX, CurrentY - 1] == null || !IsFromSameTeam(board[CurrentX, CurrentY - 1]))
                    r.Add(new Vector2Int(CurrentX, CurrentY - 1));
            return r;
        }

        /*
        public override ISpecialMove GetSpecialMove(ref ChessPiece[,] board, ref List<Vector2Int[]> moveList, ref List<Vector2Int> availableMoves)
        {
            ISpecialMove r = new NoneSpecialMove(); 
            int ourY = (team == 0) ? 0 : 7;
            var kingMove = moveList.Find(m => m[0].x == 4 && m[0].y == ourY);
            var leftRook = moveList.Find(m => m[0].x == 0 && m[0].y == ourY);
            var rightRook = moveList.Find(m => m[0].x == 7 && m[0].y == ourY);
            if (kingMove == null && CurrentX == 4)
            {
                if (team == 0)
                {
                    if (leftRook == null)
                        if (board[0, ourY].chessType == ChessPieceType.Rook)
                            if (board[0, ourY].team == Team.White)
                                if (board[3, ourY] == null && board[2, ourY] == null && board[1, ourY] == null && gameFenDataSO.GameFenData.CanWhiteCastleQueenSide)
                                {
                                    availableMoves.Add(new Vector2Int(2, ourY));
                                    r = new Castling();
                                }
                    if (rightRook == null)
                        if (board[7, ourY].chessType == ChessPieceType.Rook)
                            if (board[7, ourY].team == Team.White   )
                                if (board[5, ourY] == null && board[6, ourY] == null && gameFenDataSO.GameFenData.CanWhiteCastleKingSide)
                                {
                                    availableMoves.Add(new Vector2Int(6, ourY));
                                    r = new Castling();
                                }
                }
                else
                {
                    if (leftRook == null)
                        if (board[0, ourY].chessType == ChessPieceType.Rook)
                            if (board[0, ourY].team == Team.Black)
                                if (board[3, ourY] == null && board[2, ourY] == null && board[1, ourY] == null && gameFenDataSO.GameFenData.CanBlackCastleQueenSide)
                                {
                                    availableMoves.Add(new Vector2Int(2, ourY));
                                    r = new Castling();
                                }
                    if (rightRook == null)
                        if (board[7, ourY].chessType == ChessPieceType.Rook)
                            if (board[7, ourY].team == Team.Black)
                                if (board[5, ourY] == null && board[6, ourY] == null && gameFenDataSO.GameFenData.CanBlackCastleKingSide)
                                {
                                    availableMoves.Add(new Vector2Int(6, ourY));
                                    r = new Castling();
                                }

                }
            }

            return r;
        }
        */

        
    }
}