using System;

namespace SteampunkChess
{
    [Serializable]
    public abstract class NotationString
    {

        public string NotationStringText { get; }
        protected readonly string _notationString;

        protected NotationString(string notationString)
        {
            _notationString = notationString;
            NotationStringText = notationString;
        }

        public abstract PieceArrangementData GameDataFromNotationString();
    }
}