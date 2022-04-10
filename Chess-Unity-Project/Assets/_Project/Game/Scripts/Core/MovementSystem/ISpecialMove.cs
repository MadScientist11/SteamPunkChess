using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace SteampunkChess
{
    public interface ISpecialMove
    {  
         Task ProcessSpecialMove();
        
    }
}