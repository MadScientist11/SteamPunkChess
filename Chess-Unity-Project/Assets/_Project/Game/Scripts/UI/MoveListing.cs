using System.Collections.Generic;
using UnityEngine;

namespace SteampunkChess
{
    public class MoveListing
    {
        private readonly IReadOnlyList<Movement> _moveHistory;
        private readonly List<MoveListingEntry> _moveListingEntries;
        private readonly MoveListingData _moveListingData;
        

        public MoveListing(MoveListingData moveListingData, List<Movement> moveHistory)
        {
            _moveHistory = moveHistory;
            _moveListingEntries = new List<MoveListingEntry>();
            _moveListingData = moveListingData;
        }
        
        public void UpdateMoveListing()
        {
            var lastMove = _moveHistory[_moveHistory.Count - 1];
            if (_moveListingEntries.Count == 0 || _moveListingEntries[_moveListingEntries.Count - 1].IsFilled)
            {
                var listingEntry = CreateMoveListingEntry();
                _moveListingEntries.Add(listingEntry);
                listingEntry.AddMove(lastMove);
            }
            else
            {
                _moveListingEntries[_moveListingEntries.Count - 1].AddMove(lastMove);
            }
        }

        private MoveListingEntry CreateMoveListingEntry()
        {
            GameObject entryPrefab = (_moveHistory.Count & 1) == 0
                ? _moveListingData.moveListingDarker
                : _moveListingData.moveListingLighter;
            int fullMoveNumber = 1 + (_moveHistory.Count >> 1);
            
            MoveListingEntry listingEntry = new MoveListingEntry(entryPrefab, _moveListingData.content, fullMoveNumber);
            return listingEntry;
        }
    }
}