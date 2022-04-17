using System;

namespace SteampunkChess
{
    [Serializable]
    public class FenNotationString : NotationString
    {
        public FenNotationString(string notationString) : base(notationString)
        {
        }

        public override PieceArrangementData GameDataFromNotationString()
        {
            return FenUtility.GameDataFromStringFen(_notationString);
        }
    }
}