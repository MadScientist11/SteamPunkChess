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
        public byte OnMoveToCode = 2;
        
        public event Action<Vector2> OnSelectAndShowAvailableMovesEvent;
        public byte OnSelectAndShowAvailableMovesCode = 3;


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
            RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = receivers }; // You would have to set the Receivers to All in order to receive this event on the local client as well
            PhotonNetwork.RaiseEvent(rpcCode, content, raiseEventOptions, sendOptions);
        }

        public void OnEvent(EventData photonEvent)
        {
            if (photonEvent.Code == OnMoveToCode)
            {
                object[] data = (object[]) photonEvent.CustomData;
                OnMoveToEvent?.Invoke((Vector2)data[0]);
            }
            else if (photonEvent.Code == OnSelectAndShowAvailableMovesCode)
            {
                object[] data = (object[]) photonEvent.CustomData;
                OnSelectAndShowAvailableMovesEvent?.Invoke((Vector2)data[0]);
            }
        }
    }
}