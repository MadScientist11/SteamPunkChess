using System.Collections.Generic;
using SteampunkChess;
using UnityEngine;

namespace SteamPunkChess
{
    public class Pawn : ChessPiece
    {
        public override List<Movement> GetAvailableMoves(PieceArrangement pieceArrangement, int tileCountX, int tileCountY)
        {
            //one in front
            List<Movement> r = new List<Movement>();
            int direction = Team == 0 ? 1 : -1;
            if (CurrentY + direction < tileCountY)
            {
                if (pieceArrangement[CurrentX, CurrentY + direction] == null)
                    r.Add(new Movement(new Vector2Int(CurrentX, CurrentY), new Vector2Int(CurrentX, CurrentY + direction), pieceArrangement));

                // two in front
                if (pieceArrangement[CurrentX, CurrentY + direction] == null)
                {
                    if (Team == Team.White && CurrentY == 1 && pieceArrangement[CurrentX, CurrentY + direction * 2] == null)
                        r.Add(new Movement(new Vector2Int(CurrentX, CurrentY), new Vector2Int(CurrentX, CurrentY + direction * 2), pieceArrangement));

                    if (Team == Team.Black && CurrentY == 6 && pieceArrangement[CurrentX, CurrentY + direction * 2] == null)
                        r.Add(new Movement(new Vector2Int(CurrentX, CurrentY), new Vector2Int(CurrentX, CurrentY + direction * 2), pieceArrangement));

                }


                //Diagonal move
                if (CurrentX != tileCountX - 1)
                    if (pieceArrangement[CurrentX + 1, CurrentY + direction] != null && !IsFromSameTeam(pieceArrangement[CurrentX + 1, CurrentY + direction]))
                        r.Add(new Movement(new Vector2Int(CurrentX, CurrentY), new Vector2Int(CurrentX + 1, CurrentY + direction), pieceArrangement));
                if (CurrentX != 0)
                    if (pieceArrangement[CurrentX - 1, CurrentY + direction] != null && !IsFromSameTeam(pieceArrangement[CurrentX - 1, CurrentY + direction]))
                        r.Add(new Movement(new Vector2Int(CurrentX, CurrentY), new Vector2Int(CurrentX - 1, CurrentY + direction), pieceArrangement));
            }
       
            return r;
        }
        
        public override void GetSpecialMove(PieceArrangement pieceArrangement, List<Movement> moveHistory, List<Movement> availableMoves)
        {
            int direction = Team == 0 ? 1 : -1;
            if (Team == Team.White && CurrentY == 6 || Team == Team.Black && CurrentY == 1)
            {
                int promotionMoveIndex = moveHistory.FindIndex(x => x.Destination.y == 7 || x.Destination.y == 0);
                moveHistory[promotionMoveIndex] = new Movement(new Vector2Int(CurrentX, CurrentY),
                    new Vector2Int(moveHistory[promotionMoveIndex].Destination.x, moveHistory[promotionMoveIndex].Destination.y), pieceArrangement);
                //return new Promotion();
            }
                


            if (moveHistory.Count > 0)
            {
                var lastMove = moveHistory[moveHistory.Count - 1];
                if (pieceArrangement[lastMove.Destination.x, lastMove.Destination.y].ChessType == ChessPieceType.Pawn)
                {
                    if (Mathf.Abs(lastMove.Destination.y - lastMove.Start.y) == 2)
                    {
                        if (!IsFromSameTeam(pieceArrangement[lastMove.Destination.x, lastMove.Destination.y]))
                        {
                            if (lastMove.Destination.y == CurrentY)
                            {
                                Debug.Log("7");
                                if (lastMove.Destination.x == CurrentX - 1)
                                {
                                    Debug.Log("7");
                                    var enPassant = new EnPassant(moveHistory, pieceArrangement);
                                    var movement = new Movement(new Vector2Int(CurrentX, CurrentY),
                                        new Vector2Int(CurrentX - 1, CurrentY + direction), pieceArrangement);
                                    movement.SpecialMove = enPassant;
                                    availableMoves.Add(movement);
                                    
                                }
                                if (lastMove.Destination.x == CurrentX + 1)
                                {
                                    Debug.Log("7");
                                    var enPassant = new EnPassant(moveHistory, pieceArrangement);
                                    Movement movement = new Movement(new Vector2Int(CurrentX, CurrentY),
                                        new Vector2Int(CurrentX + 1, CurrentY + direction), pieceArrangement);
                                    movement.SpecialMove = enPassant;
                                    availableMoves.Add(movement);
                                }
                            }
                        }
                    }
                }
            }
            //return new NoneSpecialMove();
        }
    }
}