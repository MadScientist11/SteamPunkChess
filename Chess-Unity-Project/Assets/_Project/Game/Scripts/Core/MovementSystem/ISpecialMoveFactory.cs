using System.Collections.Generic;

namespace SteampunkChess
{
    public interface ISpecialMoveFactory
    {
        ISpecialMove CreateSpecialMove(SpecialMoveType specialMoveType, List<Movement> moveList,
            PieceArrangement pieceArrangement);
    }
}