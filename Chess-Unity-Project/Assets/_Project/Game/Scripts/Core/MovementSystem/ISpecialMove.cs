using System.Collections.Generic;
using UnityEngine;

namespace SteampunkChess
{
    public interface ISpecialMove
    {  
         void ProcessSpecialMove();
         //SpecialMoveStringBuilder GetStringBuilder(MoveInfo moveInfo);
    }
}