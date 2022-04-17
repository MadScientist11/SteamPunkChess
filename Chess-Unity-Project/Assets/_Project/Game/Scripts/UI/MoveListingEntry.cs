using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace SteampunkChess
{
    public class MoveListingEntry
    {
        private readonly List<Movement> _moves;
        private readonly GameObject _entryPrefab;
        private readonly Transform _entryParent;
        private readonly int _moveNumber;
        private TextMeshProUGUI _listingEntryText;

        public MoveListingEntry(GameObject entryPrefab, Transform entryParent, int moveNumber)
        {
            _moves = new List<Movement>(2);
            _entryPrefab = entryPrefab;
            _entryParent = entryParent;
            _moveNumber = moveNumber;
        }

        public bool IsFilled => _moves.Count >= 2;

        private GameObject CreateVisual()
        {
            return Object.Instantiate(_entryPrefab, _entryParent);
        }

        public void AddMove(Movement move)
        {
            if (_moves.Count == 0)
            {
                GameObject listingEntryGO = CreateVisual();
                _listingEntryText = listingEntryGO.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
                _listingEntryText.text = $"{_moveNumber}. {move.GetMovePGN()}";
            }
            else
            {
                _listingEntryText.text = $"{_listingEntryText.text} {move.GetMovePGN()}";
            }

            _moves.Add(move);
        }
    }
}