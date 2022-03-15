using System.Collections.Generic;

namespace SteampunkChess
{
    public class NoneSpecialMove : ISpecialMove
    {
        public void ProcessSpecialMove()
        {
            throw new System.NotImplementedException();
        }
        
        //public SpecialMoveStringBuilder GetStringBuilder(MoveInfo moveInfo)
        //{
        //    return new NoneSpecialMoveStringBuilder(moveInfo);
        //}
       
    }
}