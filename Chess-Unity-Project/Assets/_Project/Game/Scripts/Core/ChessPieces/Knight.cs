using System.Collections.Generic;
using SteampunkChess;
using UnityEngine;

namespace SteamPunkChess
{
    public class Knight : ChessPiece
    {

        public override List<Movement> GetAvailableMoves(PieceArrangement pieceArrangement, int tileCountX, int tileCountY, List<Movement> moveHistory)
        {
            List<Movement> r = new List<Movement>();

            // Top right
            int x = CurrentX + 1;
            int y = CurrentY + 2;
            if (x < tileCountX && y < tileCountY)
                if (pieceArrangement[x, y] == null || !IsFromSameTeam(pieceArrangement[x, y]))
                    r.Add(new Movement(new Vector2Int(CurrentX, CurrentY), new Vector2Int(x, y), pieceArrangement));

            x = CurrentX + 2;
            y = CurrentY + 1;
            if (x < tileCountX && y < tileCountY)
                if (pieceArrangement[x, y] == null || !IsFromSameTeam(pieceArrangement[x, y]))
                    r.Add(new Movement(new Vector2Int(CurrentX, CurrentY), new Vector2Int(x, y), pieceArrangement));

            // Top left
            x = CurrentX - 1;
            y = CurrentY + 2;
            if (x >= 0 && y < tileCountY)
                if (pieceArrangement[x, y] == null || !IsFromSameTeam(pieceArrangement[x, y]))
                    r.Add(new Movement(new Vector2Int(CurrentX, CurrentY), new Vector2Int(x, y), pieceArrangement));

            x = CurrentX - 2;
            y = CurrentY + 1;
            if (x >= 0 && y < tileCountY)
                if (pieceArrangement[x, y] == null || !IsFromSameTeam(pieceArrangement[x, y]))
                    r.Add(new Movement(new Vector2Int(CurrentX, CurrentY), new Vector2Int(x, y), pieceArrangement));


            // Bottom right
            x = CurrentX + 1;
            y = CurrentY - 2;
            if (x < tileCountX && y >= 0)
                if (pieceArrangement[x, y] == null || !IsFromSameTeam(pieceArrangement[x, y]))
                    r.Add(new Movement(new Vector2Int(CurrentX, CurrentY), new Vector2Int(x, y), pieceArrangement));

            x = CurrentX + 2;
            y = CurrentY - 1;
            if (x < tileCountX && y >= 0)
                if (pieceArrangement[x, y] == null || !IsFromSameTeam(pieceArrangement[x, y]))
                    r.Add(new Movement(new Vector2Int(CurrentX, CurrentY), new Vector2Int(x, y), pieceArrangement));

            // Bottom left
            x = CurrentX - 1;
            y = CurrentY - 2;
            if (x >= 0 && y >= 0)
                if (pieceArrangement[x, y] == null || !IsFromSameTeam(pieceArrangement[x, y]))
                    r.Add(new Movement(new Vector2Int(CurrentX, CurrentY), new Vector2Int(x, y), pieceArrangement));

            x = CurrentX - 2;
            y = CurrentY - 1;
            if (x >= 0 && y >= 0)
                if (pieceArrangement[x, y] == null || !IsFromSameTeam(pieceArrangement[x, y]))
                    r.Add(new Movement(new Vector2Int(CurrentX, CurrentY), new Vector2Int(x, y), pieceArrangement));
            return r;
        }
    }
}