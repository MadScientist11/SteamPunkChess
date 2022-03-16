using System.Collections.Generic;
using SteampunkChess;
using UnityEngine;

namespace SteamPunkChess
{
    public static class FenUtility
    {
        private static readonly Dictionary<char, PieceInfo> fenMappings = new Dictionary<char, PieceInfo>()
        {
            { 'K', new PieceInfo(ChessPieceType.King, Team.White) },
            { 'k', new PieceInfo(ChessPieceType.King, Team.Black) },
            { 'Q', new PieceInfo(ChessPieceType.Queen, Team.White) },
            { 'q', new PieceInfo(ChessPieceType.Queen, Team.Black) },
            { 'R', new PieceInfo(ChessPieceType.Rook, Team.White) },
            { 'r', new PieceInfo(ChessPieceType.Rook, Team.Black) },
            { 'B', new PieceInfo(ChessPieceType.Bishop, Team.White) },
            { 'b', new PieceInfo(ChessPieceType.Bishop, Team.Black) },
            { 'N', new PieceInfo(ChessPieceType.Knight, Team.White)},
            { 'n', new PieceInfo(ChessPieceType.Knight, Team.Black) },
            { 'P', new PieceInfo(ChessPieceType.Pawn, Team.White) },
            { 'p', new PieceInfo(ChessPieceType.Pawn, Team.Black) },
        };

        public static string FenStringFromGameFenData()
        {
            return "";
        }
        
        public static GameDataFen GameFenDataFromStringFen(string fen)
        {
            var data = new GameDataFen();
            data.parseFenError = "";
        
            string[] parts = fen.Split(' ');
            if (parts.Length != 6)
            {
                data.parseFenError = "The FEN string has too much, or too few, parts.";

                return data;
            }
            string[] rows = parts[0].Split('/');
            if (rows.Length != 8)
            {
                data.parseFenError = "The board in the FEN string has an invalid number of rows.";
                return data;
            }


            data.piecesInfo = BoardArrangementFromFen(rows);

            if (parts[1] == "w")
            {
                data.WhoseTurn = 0;
            }
            else if (parts[1] == "b")
            {
                data.WhoseTurn = 1;
            }
            else
            {
                data.parseFenError = "Expected `w` or `b` for the active player in the FEN string.";
                return data;
            }

            if (parts[2].Contains("K")) data.CanWhiteCastleKingSide = true;
            else data.CanWhiteCastleKingSide = false;

            if (parts[2].Contains("Q")) data.CanWhiteCastleQueenSide = true;
            else data.CanWhiteCastleQueenSide = false;

            if (parts[2].Contains("k")) data.CanBlackCastleKingSide = true;
            else data.CanBlackCastleKingSide = false;

            if (parts[2].Contains("q")) data.CanBlackCastleQueenSide = true;
            else data.CanBlackCastleQueenSide = false;

            if (parts[3] == "-") data.EnPassant = Vector2Int.zero;
            else
            {
                char[] charArray = parts[3].ToCharArray();
                int column = 0;
                int row = 0;
                switch (charArray[0])
                {
                    case 'a':
                        column = 0;
                        break;
                    case 'b':
                        column = 1;
                        break;
                    case 'c':
                        column = 2;
                        break;
                    case 'd':
                        column = 3;
                        break;
                    case 'e':
                        column = 4;
                        break;
                    case 'f':
                        column = 5;
                        break;
                    case 'g':
                        column = 6;
                        break;
                    case 'h':
                        column = 7;
                        break;


                    default:
                        data.parseFenError = "Invalid en passant field in FEN.";
                        return data;

                }
                row = (int)char.GetNumericValue(charArray[1]) - 1;
                if (row == -1)
                {
                    data.parseFenError = "Invalid en passant field in FEN.";
                    return data;
                }
                data.EnPassant = new Vector2Int(column, row);
            }
            int halfmoveClock;
            if (int.TryParse(parts[4], out halfmoveClock))
            {
                data.HalfMoveClock = halfmoveClock;
            }
            else
            {
                data.parseFenError = "Halfmove clock in FEN is invalid.";
                return data;
            }

            int fullMoveNumber;
            if (int.TryParse(parts[5], out fullMoveNumber))
            {
                data.FullMoveNumber = fullMoveNumber;
            }
            else
            {
                data.parseFenError = "Fullmove number in FEN is invalid.";
                return data;
            }

            return data;
        }
        public static PieceInfo[][] BoardArrangementFromFen(string[] rows)
        {
            var pieceArray = new PieceInfo[8][];
            for (int j = 0; j < 8; j++)
            {
                pieceArray[j] = new PieceInfo[8] { null, null, null, null, null, null, null, null };
            }
            for (int i = 0; i < rows.Length; i++)
            {
                string row = rows[i];
                int j = 0;
                foreach (char rowSymb in row)
                {
                    if (char.IsDigit(rowSymb))
                    {
                        j += (int)char.GetNumericValue(rowSymb);
                        continue;
                    }
                    pieceArray[j][7 - i] = fenMappings[rowSymb];

                    j++;
                }

            }
            return pieceArray;
        }

    }
}
