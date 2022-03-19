using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace SteampunkChess
{
    public static class FenUtility
    {
        private static readonly Dictionary<char, PieceInfo> FenMappings = new Dictionary<char, PieceInfo>()
        {
            {'K', new PieceInfo(ChessPieceType.King, Team.White)},
            {'k', new PieceInfo(ChessPieceType.King, Team.Black)},
            {'Q', new PieceInfo(ChessPieceType.Queen, Team.White)},
            {'q', new PieceInfo(ChessPieceType.Queen, Team.Black)},
            {'R', new PieceInfo(ChessPieceType.Rook, Team.White)},
            {'r', new PieceInfo(ChessPieceType.Rook, Team.Black)},
            {'B', new PieceInfo(ChessPieceType.Bishop, Team.White)},
            {'b', new PieceInfo(ChessPieceType.Bishop, Team.Black)},
            {'N', new PieceInfo(ChessPieceType.Knight, Team.White)},
            {'n', new PieceInfo(ChessPieceType.Knight, Team.Black)},
            {'P', new PieceInfo(ChessPieceType.Pawn, Team.White)},
            {'p', new PieceInfo(ChessPieceType.Pawn, Team.Black)},
        };

        private static readonly Dictionary<PieceInfo, char> FenCharMappings = new Dictionary<PieceInfo, char>()
        {
            {new PieceInfo(ChessPieceType.King, Team.White), 'K'},
            {new PieceInfo(ChessPieceType.King, Team.Black), 'k'},
            {new PieceInfo(ChessPieceType.Queen, Team.White), 'Q'},
            {new PieceInfo(ChessPieceType.Queen, Team.Black), 'q'},
            {new PieceInfo(ChessPieceType.Rook, Team.White), 'R'},
            {new PieceInfo(ChessPieceType.Rook, Team.Black), 'r'},
            {new PieceInfo(ChessPieceType.Bishop, Team.White), 'B'},
            {new PieceInfo(ChessPieceType.Bishop, Team.Black), 'b'},
            {new PieceInfo(ChessPieceType.Knight, Team.White), 'N'},
            {new PieceInfo(ChessPieceType.Knight, Team.Black), 'n'},
            {new PieceInfo(ChessPieceType.Pawn, Team.White), 'P'},
            {new PieceInfo(ChessPieceType.Pawn, Team.Black), 'p'},
        };


        public static string FenStringFromGameData(PieceArrangementData pieceArrangementData)
        {
            var fenBuilder = new StringBuilder();
            PieceInfo[,] board = pieceArrangementData.piecesInfo;
            int dimension = board.GetLength(0);
            
            
            for (int i = 0; i < dimension; i++)
            {
                PieceInfo[] row = board.GetColumn(7 - i);
                
                int empty = 0;
                foreach (PieceInfo piece in row)
                {
                    char pieceChar = piece == null ? '\0' : FenCharMappings[piece];
                    if (pieceChar == '\0')
                    {
                        empty++;
                        continue;
                    }

                    if (empty != 0)
                    {
                        fenBuilder.Append(empty);
                        empty = 0;
                    }

                    fenBuilder.Append(pieceChar);
                }

                if (empty != 0)
                {
                    fenBuilder.Append(empty);
                }

                if (i != dimension - 1)
                {
                    fenBuilder.Append('/');
                }
            }

            fenBuilder.Append(' ');

            fenBuilder.Append(pieceArrangementData.whoseTurn == (int)Team.White ? 'w' : 'b');

            fenBuilder.Append(' ');

            bool hasAnyCastlingOptions = false;


            if (pieceArrangementData.canWhiteCastleKingSide)
            {
                fenBuilder.Append('K');
                hasAnyCastlingOptions = true;
            }

            if (pieceArrangementData.canWhiteCastleQueenSide)
            {
                fenBuilder.Append('Q');
                hasAnyCastlingOptions = true;
            }


            if (pieceArrangementData.canBlackCastleKingSide)
            {
                fenBuilder.Append('k');
                hasAnyCastlingOptions = true;
            }

            if (pieceArrangementData.canBlackCastleQueenSide)
            {
                fenBuilder.Append('q');
                hasAnyCastlingOptions = true;
            }

            if (!hasAnyCastlingOptions)
            {
                fenBuilder.Append('-');
            }

            fenBuilder.Append(' ');

            //TODO: En Passant coords
            //DetailedMove last;
            //if (Moves.Count > 0 && (last = Moves[Moves.Count - 1]).Piece is Pawn &&
            //    Math.Abs(last.OriginalPosition.Rank - last.NewPosition.Rank) == 2
            //    && last.OriginalPosition.Rank == (last.Player == Player.White ? 2 : 7))
            //{
            //    fenBuilder.Append(last.NewPosition.File.ToString().ToLowerInvariant());
            //    fenBuilder.Append(last.Player == Player.White ? 3 : 6);
            //}
            //else
            //{
                fenBuilder.Append("-");
           // }

            fenBuilder.Append(' ');

            fenBuilder.Append(pieceArrangementData.halfMoveClock);

            fenBuilder.Append(' ');

            fenBuilder.Append(pieceArrangementData.fullMoveNumber);

            return fenBuilder.ToString();
        }

        public static PieceArrangementData GameDataFromStringFen(string fen)
        {
            var data = new PieceArrangementData();
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
            Debug.Log(parts[1]);

            if (parts[1] == "w")
            {
                data.whoseTurn = 0;
            }
            else if (parts[1] == "b")
            {
                data.whoseTurn = 1;
            }
            else
            {
                data.parseFenError = "Expected `w` or `b` for the active player in the FEN string.";
                return data;
            }

            if (parts[2].Contains("K")) data.canWhiteCastleKingSide = true;
            else data.canWhiteCastleKingSide = false;

            if (parts[2].Contains("Q")) data.canWhiteCastleQueenSide = true;
            else data.canWhiteCastleQueenSide = false;

            if (parts[2].Contains("k")) data.canBlackCastleKingSide = true;
            else data.canBlackCastleKingSide = false;

            if (parts[2].Contains("q")) data.canBlackCastleQueenSide = true;
            else data.canBlackCastleQueenSide = false;

            if (parts[3] == "-") data.enPassant = Vector2Int.zero;
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

                row = (int) char.GetNumericValue(charArray[1]) - 1;
                if (row == -1)
                {
                    data.parseFenError = "Invalid en passant field in FEN.";
                    return data;
                }

                data.enPassant = new Vector2Int(column, row);
            }

            if (int.TryParse(parts[4], out var halfmoveClock))
            {
                data.halfMoveClock = halfmoveClock;
            }
            else
            {
                data.parseFenError = "Halfmove clock in FEN is invalid.";
                return data;
            }

            if (int.TryParse(parts[5], out var fullMoveNumber))
            {
                data.fullMoveNumber = fullMoveNumber;
            }
            else
            {
                data.parseFenError = "Fullmove number in FEN is invalid.";
                return data;
            }

            return data;
        }

        private static PieceInfo[,] BoardArrangementFromFen(string[] rows)
        {
            var pieceArray = new PieceInfo[8, 8];
    
            for (int i = 0; i < rows.Length; i++)
            {
                string row = rows[i];
                int j = 0;
                foreach (char rowSymb in row)
                {
                    if (char.IsDigit(rowSymb))
                    {
                        j += (int) char.GetNumericValue(rowSymb);
                        continue;
                    }

                    pieceArray[j, 7 - i] = FenMappings[rowSymb];

                    j++;
                }
            }

            return pieceArray;
        }
    }
}