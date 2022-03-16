using System.Collections.Generic;
using SteampunkChess;
using UnityEngine;


namespace SteamPunkChess
{
    public class King : ChessPiece
    {
        //[SerializeField] private SOGameFenDataSO gameFenDataSO;
      
        public override List<Movement> GetAvailableMoves(PieceArrangement pieceArrangement, int tileCountX, int tileCountY, List<Movement> moveHistory, List<Movement> availableMoves)
        {
            List<Movement> r = new List<Movement>();

            // Right
            if (CurrentX + 1 < tileCountX)
            {
                if (pieceArrangement[CurrentX + 1, CurrentY] == null)
                    r.Add(new Movement(new Vector2Int(CurrentX, CurrentY), new Vector2Int(CurrentX + 1, CurrentY), pieceArrangement));
                else if (!IsFromSameTeam(pieceArrangement[CurrentX + 1, CurrentY]))
                    r.Add(new Movement(new Vector2Int(CurrentX, CurrentY), new Vector2Int(CurrentX + 1, CurrentY), pieceArrangement));

                // Top right
                if (CurrentY + 1 < tileCountY)
                    if (pieceArrangement[CurrentX + 1, CurrentY + 1] == null)
                        r.Add(new Movement(new Vector2Int(CurrentX, CurrentY), new Vector2Int(CurrentX + 1, CurrentY + 1), pieceArrangement));
                    else if (!IsFromSameTeam(pieceArrangement[CurrentX + 1, CurrentY + 1]))
                        r.Add(new Movement(new Vector2Int(CurrentX, CurrentY), new Vector2Int(CurrentX + 1, CurrentY + 1), pieceArrangement));

                // Bottom right
                if (CurrentY - 1 >= 0)
                    if (pieceArrangement[CurrentX + 1, CurrentY - 1] == null)
                        r.Add(new Movement(new Vector2Int(CurrentX, CurrentY), new Vector2Int(CurrentX + 1, CurrentY - 1), pieceArrangement));
                    else if (!IsFromSameTeam(pieceArrangement[CurrentX + 1, CurrentY - 1]))
                        r.Add(new Movement(new Vector2Int(CurrentX, CurrentY), new Vector2Int(CurrentX + 1, CurrentY - 1), pieceArrangement));
            }
            // Left
            if (CurrentX - 1 >= 0)
            {
                if (pieceArrangement[CurrentX - 1, CurrentY] == null)
                    r.Add(new Movement(new Vector2Int(CurrentX, CurrentY), new Vector2Int(CurrentX - 1, CurrentY), pieceArrangement));
                else if (!IsFromSameTeam(pieceArrangement[CurrentX - 1, CurrentY]))
                    r.Add(new Movement(new Vector2Int(CurrentX, CurrentY), new Vector2Int(CurrentX - 1, CurrentY), pieceArrangement));

                // Top right
                if (CurrentY + 1 < tileCountY)
                    if (pieceArrangement[CurrentX - 1, CurrentY + 1] == null)
                        r.Add(new Movement(new Vector2Int(CurrentX, CurrentY), new Vector2Int(CurrentX - 1, CurrentY + 1), pieceArrangement));
                    else if (!IsFromSameTeam(pieceArrangement[CurrentX - 1, CurrentY + 1]))
                        r.Add(new Movement(new Vector2Int(CurrentX, CurrentY), new Vector2Int(CurrentX - 1, CurrentY + 1), pieceArrangement));

                // Bottom right
                if (CurrentY - 1 >= 0)
                    if (pieceArrangement[CurrentX - 1, CurrentY - 1] == null)
                        r.Add(new Movement(new Vector2Int(CurrentX, CurrentY), new Vector2Int(CurrentX - 1, CurrentY - 1), pieceArrangement));
                    else if (!IsFromSameTeam(pieceArrangement[CurrentX - 1, CurrentY - 1]))
                        r.Add(new Movement(new Vector2Int(CurrentX, CurrentY), new Vector2Int(CurrentX - 1, CurrentY - 1), pieceArrangement));
            }
            //Up
            if (CurrentY + 1 < tileCountY)
                if (pieceArrangement[CurrentX, CurrentY + 1] == null || !IsFromSameTeam(pieceArrangement[CurrentX, CurrentY + 1]))
                    r.Add(new Movement(new Vector2Int(CurrentX, CurrentY), new Vector2Int(CurrentX, CurrentY + 1), pieceArrangement));
            //Down
            if (CurrentY - 1 >= 0)
                if (pieceArrangement[CurrentX, CurrentY - 1] == null || !IsFromSameTeam(pieceArrangement[CurrentX, CurrentY - 1]))
                    r.Add(new Movement(new Vector2Int(CurrentX, CurrentY), new Vector2Int(CurrentX, CurrentY - 1), pieceArrangement));

            UpdateWithSpecialMove(pieceArrangement, moveHistory, r);
            return r;
        }

        public override void UpdateWithSpecialMove(PieceArrangement pieceArrangement, List<Movement> moveHistory, List<Movement> availableMoves)
        {
           
            int ourY = (Team == 0) ? 0 : 7;
            var kingMove = moveHistory.Find(m => m.Start.x == 4 && m.Start.y == ourY);
            var leftRook = moveHistory.Find(m => m.Start.x == 0 && m.Start.y == ourY);
            var rightRook = moveHistory.Find(m => m.Start.x == 7 && m.Start.y == ourY);
            if (kingMove == null && CurrentX == 4)
            {
                if (Team == 0)
                {
                    if (leftRook == null)
                        if (pieceArrangement[0, ourY].ChessType == ChessPieceType.Rook)
                            if (pieceArrangement[0, ourY].Team == Team.White)
                                if (pieceArrangement[3, ourY] == null && pieceArrangement[2, ourY] == null && pieceArrangement[1, ourY] == null)
                                {
                                    var movement = new Movement(new Vector2Int(CurrentX, CurrentY),
                                        new Vector2Int(2, ourY), 
                                        pieceArrangement, 
                                        new Castling(moveHistory, pieceArrangement));
                                    availableMoves.Add(movement);
                                     
                                }
                    if (rightRook == null)
                        if (pieceArrangement[7, ourY].ChessType == ChessPieceType.Rook)
                            if (pieceArrangement[7, ourY].Team == Team.White   )
                                if (pieceArrangement[5, ourY] == null && pieceArrangement[6, ourY] == null)
                                {
                                    var movement = new Movement(new Vector2Int(CurrentX, CurrentY),
                                        new Vector2Int(6, ourY), 
                                        pieceArrangement, 
                                        new Castling(moveHistory, pieceArrangement));
                                    availableMoves.Add(movement);
                                   
                                }
                }
                else
                {
                    if (leftRook == null)
                        if (pieceArrangement[0, ourY].ChessType == ChessPieceType.Rook)
                            if (pieceArrangement[0, ourY].Team == Team.Black)
                                if (pieceArrangement[3, ourY] == null && pieceArrangement[2, ourY] == null && pieceArrangement[1, ourY] == null)
                                {
                                    var movement = new Movement(new Vector2Int(CurrentX, CurrentY),
                                        new Vector2Int(2, ourY), 
                                        pieceArrangement, 
                                        new Castling(moveHistory, pieceArrangement));
                                    availableMoves.Add(movement);
                                }
                    if (rightRook == null)
                        if (pieceArrangement[7, ourY].ChessType == ChessPieceType.Rook)
                            if (pieceArrangement[7, ourY].Team == Team.Black)
                                if (pieceArrangement[5, ourY] == null && pieceArrangement[6, ourY] == null)
                                {
                                    var movement = new Movement(new Vector2Int(CurrentX, CurrentY),
                                        new Vector2Int(6, ourY), 
                                        pieceArrangement, 
                                        new Castling(moveHistory, pieceArrangement));
                                    availableMoves.Add(movement);
                                }

                }
            }
     
        }
        

        
    }
}