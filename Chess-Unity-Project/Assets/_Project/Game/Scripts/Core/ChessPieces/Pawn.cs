using System.Collections.Generic;
using UnityEngine;

namespace SteampunkChess
{
    public class Pawn : ChessPiece
    {
        private readonly ISpecialMoveFactory _specialMoveFactory;

        public Pawn(ISpecialMoveFactory specialMoveFactory)
        {
            _specialMoveFactory = specialMoveFactory;
        }
        
        public override List<Movement> GetAvailableMoves(PieceArrangement pieceArrangement, int tileCountX, int tileCountY, List<Movement> moveHistory)
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

            UpdateWithSpecialMove(pieceArrangement, moveHistory, r);
            return r;
        }
        
        private void UpdateWithSpecialMove(PieceArrangement pieceArrangement, List<Movement> moveHistory, List<Movement> availableMoves)
        {
            int direction = Team == 0 ? 1 : -1;
            if (Team == Team.White && CurrentY == 6 || Team == Team.Black && CurrentY == 1)
            {
               
                List<int> promotionMoveIndexes = availableMoves.FindAllIndexes(x => x.Destination.y == 7 || x.Destination.y == 0);
                foreach (var index in promotionMoveIndexes)
                {
                    var promotion = _specialMoveFactory.CreateSpecialMove(SpecialMoveType.Promotion, moveHistory, pieceArrangement);
                    availableMoves[index] = new Movement(new Vector2Int(CurrentX, CurrentY),
                        new Vector2Int(availableMoves[index].Destination.x,
                            availableMoves[index].Destination.y),
                        pieceArrangement,
                        promotion
                    );
                }
            }
            
            if (moveHistory.Count <= 0) return;
            
            var lastMove = moveHistory[moveHistory.Count - 1];
            
            if (pieceArrangement[lastMove.Destination.x, lastMove.Destination.y].ChessType == ChessPieceType.Pawn)
            {
                if (Mathf.Abs(lastMove.Destination.y - lastMove.Start.y) == 2)
                {
                    if (!IsFromSameTeam(pieceArrangement[lastMove.Destination.x, lastMove.Destination.y]))
                    {
                        if (lastMove.Destination.y == CurrentY)
                        {
                                
                            if (lastMove.Destination.x == CurrentX - 1)
                            {
                                var enPassant = _specialMoveFactory.CreateSpecialMove(SpecialMoveType.EnPassant, moveHistory, pieceArrangement);
                                var movement = new Movement(new Vector2Int(CurrentX, CurrentY),
                                    new Vector2Int(CurrentX - 1, CurrentY + direction), 
                                    pieceArrangement, 
                                    enPassant);
                                   
                                availableMoves.Add(movement);
                            }
                            if (lastMove.Destination.x == CurrentX + 1)
                            {
                                var enPassant = _specialMoveFactory.CreateSpecialMove(SpecialMoveType.EnPassant, moveHistory, pieceArrangement);
                                Movement movement = new Movement(new Vector2Int(CurrentX, CurrentY),
                                    new Vector2Int(CurrentX + 1, CurrentY + direction), 
                                    pieceArrangement, 
                                    enPassant);
                                    
                                availableMoves.Add(movement);
                            }
                        }
                    }
                }
            }
        }
    }
}