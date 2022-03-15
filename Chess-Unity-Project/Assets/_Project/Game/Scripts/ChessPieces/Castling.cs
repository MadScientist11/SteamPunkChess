using System.Collections.Generic;

namespace SteampunkChess
{
    public class Castling : ISpecialMove
    {
        public void ProcessSpecialMove()
        {
            throw new System.NotImplementedException();
        }
        
        /*public void ProcessSpecialMove(ref List<Vector2Int[]> moveList, ref ChessPiece[,] chessPieces)
        {
            Vector2Int[] lastMove = moveList[moveList.Count - 1];

            Debug.Log("Castling");
            //Left rook
            if (lastMove[1].x == 2) // was king positioned?
            {
                if (lastMove[1].y == 0)
                {
                    ChessPiece rook = chessPieces[0, 0];
                    chessPieces[3, 0] = rook;
               
                    pieceCreator.PositionSinglePiece(3, 0, true);
                    Debug.Log("Castling1");

                    chessPieces[0, 0] = null;

                    
                }
                else if (lastMove[1].y == 7)
                {

                    ChessPiece rook = chessPieces[0, 7];
                    chessPieces[3, 7] = rook;
                
                    pieceCreator.PositionSinglePiece(3, 7, true);
                    Debug.Log("Castling2");
                    chessPieces[0, 7] = null;

                }
            }
            // Right rook
            else if (lastMove[1].x == 6)
            {
                if (lastMove[1].y == 0)
                {
                    Debug.Log("Castling3");
                    ChessPiece rook = chessPieces[7, 0];
                    chessPieces[5, 0] = rook;
                  
                    pieceCreator.PositionSinglePiece(5, 0, true);

                    chessPieces[7, 0] = null;

                }
                else if (lastMove[1].y == 7)
                {
                    Debug.Log("Castling4");

                    ChessPiece rook = chessPieces[7, 7];
                    chessPieces[5, 7] = rook;
             
                    pieceCreator.PositionSinglePiece(5, 7, true);

                    chessPieces[7, 7] = null;
   
                }
            }
    
        }*/
        //public SpecialMoveStringBuilder GetStringBuilder(MoveInfo moveInfo)
        //{
        //    return new CastlingSpecialMoveStringBuilder(moveInfo);
       //// }


   
    }
}