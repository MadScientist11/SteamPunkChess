using System.Threading.Tasks;

namespace SteampunkChess
{
    public interface IService
    {
        public string InitializationMessage { get; }

        public Task Initialize();
    }
}