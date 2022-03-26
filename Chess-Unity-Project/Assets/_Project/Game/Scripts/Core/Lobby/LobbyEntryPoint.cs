using System;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using SteampunkChess.NetworkService;
using UnityEngine;
using Zenject;

namespace SteampunkChess
{
    public class LobbyEntryPoint : MonoBehaviourPunCallbacks
    {
        private Lobby _lobby;
       

        [Inject]
        private void Construct(Lobby lobby)
        {
            _lobby = lobby;
        }

        private void Awake()
        {
            _lobby.Initialize();
        }

        public override void OnJoinedRoom()
        {
            Debug.LogError(PhotonNetwork.CurrentRoom.Name);
            if (PhotonNetwork.LocalPlayer.IsMasterClient)
            {
                print("MasterClient!");
                Hashtable prop = new Hashtable();
                prop["Team"] = 0;
                PhotonNetwork.LocalPlayer.SetCustomProperties(prop);

            }
            else
            {
                Debug.LogError("This is local player setting custom properties...");
                Hashtable prop = new Hashtable();
                prop["Team"] = 1;
                PhotonNetwork.LocalPlayer.SetCustomProperties(prop);
                
            }
        }
        
        public override void OnPlayerEnteredRoom(Player newPlayer)
        {
            if (PhotonNetwork.CurrentRoom.PlayerCount == 2)
            {
                PhotonNetwork.CurrentRoom.IsOpen = false;
                PhotonNetwork.LoadLevel(1);
            }
        }

     
    

      
    }
}
