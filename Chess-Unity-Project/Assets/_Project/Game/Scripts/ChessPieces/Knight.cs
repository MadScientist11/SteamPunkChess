﻿using System.Collections.Generic;
using SteampunkChess;
using UnityEngine;

namespace SteamPunkChess
{
    public class Knight : ChessPiece
    {

        public override List<Vector2Int> GetAvailableMoves(ref ChessPiece[,] board, int tileCountX, int tileCountY)
        {
            List<Vector2Int> r = new List<Vector2Int>();

            // Top right
            int x = CurrentX + 1;
            int y = CurrentY + 2;
            if (x < tileCountX && y < tileCountY)
                if (board[x, y] == null || !IsFromSameTeam(board[x, y]))
                    r.Add(new Vector2Int(x, y));

            x = CurrentX + 2;
            y = CurrentY + 1;
            if (x < tileCountX && y < tileCountY)
                if (board[x, y] == null || !IsFromSameTeam(board[x, y]))
                    r.Add(new Vector2Int(x, y));

            // Top left
            x = CurrentX - 1;
            y = CurrentY + 2;
            if (x >= 0 && y < tileCountY)
                if (board[x, y] == null || !IsFromSameTeam(board[x, y]))
                    r.Add(new Vector2Int(x, y));

            x = CurrentX - 2;
            y = CurrentY + 1;
            if (x >= 0 && y < tileCountY)
                if (board[x, y] == null || !IsFromSameTeam(board[x, y]))
                    r.Add(new Vector2Int(x, y));


            // Bottom right
            x = CurrentX + 1;
            y = CurrentY - 2;
            if (x < tileCountX && y >= 0)
                if (board[x, y] == null || !IsFromSameTeam(board[x, y]))
                    r.Add(new Vector2Int(x, y));

            x = CurrentX + 2;
            y = CurrentY - 1;
            if (x < tileCountX && y >= 0)
                if (board[x, y] == null || !IsFromSameTeam(board[x, y]))
                    r.Add(new Vector2Int(x, y));

            // Bottom left
            x = CurrentX - 1;
            y = CurrentY - 2;
            if (x >= 0 && y >= 0)
                if (board[x, y] == null || !IsFromSameTeam(board[x, y]))
                    r.Add(new Vector2Int(x, y));

            x = CurrentX - 2;
            y = CurrentY - 1;
            if (x >= 0 && y >= 0)
                if (board[x, y] == null || !IsFromSameTeam(board[x, y]))
                    r.Add(new Vector2Int(x, y));
            return r;
        }
    }
}