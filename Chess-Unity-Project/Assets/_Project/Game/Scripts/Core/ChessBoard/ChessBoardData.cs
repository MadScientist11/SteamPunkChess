namespace SteampunkChess
{
    public class ChessBoardData
    {
        public NotationString NotationString { get; }
        public ChessBoardInfoSO ChessBoardInfoSO { get; }
        public PiecesPrefabsSO PiecesPrefabsSO { get; }
        public TileSelectionInfoSO TileSelectionSO { get; }

        public ChessBoardData(NotationString notationString, ChessBoardInfoSO chessBoardInfoSO, PiecesPrefabsSO piecesPrefabsSO, TileSelectionInfoSO tileSelectionSO)
            => (NotationString, ChessBoardInfoSO, PiecesPrefabsSO, TileSelectionSO) 
                = (notationString, chessBoardInfoSO, piecesPrefabsSO, tileSelectionSO);

      
    }
}