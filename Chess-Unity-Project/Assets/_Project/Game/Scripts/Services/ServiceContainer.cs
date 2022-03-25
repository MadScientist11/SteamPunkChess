using System.Collections.Generic;

namespace SteampunkChess
{
    public class ServiceContainer
    {
        public List<IService> ServiceList { get; } = new List<IService>();
    }
}