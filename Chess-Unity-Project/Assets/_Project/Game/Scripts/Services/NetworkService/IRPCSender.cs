using ExitGames.Client.Photon;
using Photon.Realtime;

namespace SteampunkChess.NetworkService
{
    public interface IRPCSender
    {
        void SendRPC(byte rpcCode, object[] content, ReceiverGroup receivers, SendOptions sendOptions);
    }
}