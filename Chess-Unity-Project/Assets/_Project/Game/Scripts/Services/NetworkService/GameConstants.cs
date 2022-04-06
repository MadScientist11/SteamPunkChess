namespace SteampunkChess
{
    public partial class GameConstants
    {
        public static class RPCMethodsByteCodes
        {
            public const byte OnMoveToCode = 2;
            public const byte OnSelectAndShowAvailableMovesCode = 3;
            public const byte OnEndGameCode = 4;
        }
    }
}