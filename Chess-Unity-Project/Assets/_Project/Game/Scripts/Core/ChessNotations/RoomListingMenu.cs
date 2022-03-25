using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;

namespace SteampunkChess
{
    public class RoomListingMenu : MonoBehaviour
    {
        [SerializeField] private GameObject _roomListingPrefab;
        [SerializeField] private Transform _roomMenuContent;
    
        private Dictionary<string, RoomListing> _cachedRoomList = new Dictionary<string, RoomListing>();


        private RoomListing CreateRoomListing(RoomInfo roomInfo)
        {
            var roomListing = Instantiate(_roomListingPrefab, _roomMenuContent).GetComponent<RoomListing>();
            roomListing.RoomInfo = roomInfo;
            roomListing.Initialize();
            return roomListing;
        }
        
        public void UpdateMoveListings(List<RoomInfo> roomList)
        {
            for(int i=0; i<roomList.Count; i++)
            {
                RoomInfo info = roomList[i];
                if (info.RemovedFromList)
                {
                    _cachedRoomList.Remove(info.Name);
                }
                else
                {
                    _cachedRoomList[info.Name] = CreateRoomListing(info);
                }
            }
        }

        
    }
}
