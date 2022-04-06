using System;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

namespace SteampunkChess.NetworkService
{
   
    public class PhotonRPCSender : MonoBehaviour, IRPCSender, IOnEventCallback
    {
        public event Action<Vector2> OnMoveToEvent;
        public event Action<Vector2> OnSelectAndShowAvailableMovesEvent;
    
        


        private void OnEnable()
        {
            PhotonNetwork.AddCallbackTarget(this);
        }

        private void OnDisable()
        {
            PhotonNetwork.RemoveCallbackTarget(this);
            OnMoveToEvent = null;
            OnSelectAndShowAvailableMovesEvent = null;
    
        }

        public void SendRPC(byte rpcCode, object[] content, ReceiverGroup receivers, SendOptions sendOptions)
        {
            RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = receivers }; 
            PhotonNetwork.RaiseEvent(rpcCode, content, raiseEventOptions, sendOptions);
        }

        public void OnEvent(EventData photonEvent)
        {
            switch (photonEvent.Code)
            {
                case GameConstants.RPCMethodsByteCodes.OnMoveToCode:
                {
                    object[] data = (object[]) photonEvent.CustomData;
                    OnMoveToEvent?.Invoke((Vector2)data[0]);
                    break;
                }
                case GameConstants.RPCMethodsByteCodes.OnSelectAndShowAvailableMovesCode:
                {
                    object[] data = (object[]) photonEvent.CustomData;
                    OnSelectAndShowAvailableMovesEvent?.Invoke((Vector2)data[0]);
                    break;
                }
            }
        }
    }
}

