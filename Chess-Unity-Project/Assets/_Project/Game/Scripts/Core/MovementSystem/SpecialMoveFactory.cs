using System;
using System.Collections.Generic;
using Zenject;

namespace SteampunkChess
{
    public class SpecialMoveFactory : ISpecialMoveFactory
    {
        private readonly IInstantiator _instantiator;

        public SpecialMoveFactory(IInstantiator instantiator)
        {
            _instantiator = instantiator;
        }

        public ISpecialMove CreateSpecialMove(SpecialMoveType specialMoveType, List<Movement> moveList,
            PieceArrangement pieceArrangement)
        {
            return specialMoveType switch
            {
                SpecialMoveType.EnPassant => _instantiator.Instantiate<EnPassant>(new object[]
                    {moveList, pieceArrangement}),
                SpecialMoveType.Castling => _instantiator.Instantiate<Castling>(new object[]
                    {moveList, pieceArrangement}),
                SpecialMoveType.Promotion => _instantiator.Instantiate<Promotion>(new object[]
                    {moveList, pieceArrangement}),
                _ => throw new ArgumentOutOfRangeException(nameof(specialMoveType), specialMoveType, null)
            };
        }
    }
}