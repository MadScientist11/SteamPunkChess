using System.Collections.Generic;
using Photon.Realtime;
using UnityEngine;

namespace SteampunkChess
{
    public class RoomListingMenu : MonoBehaviour
    {
        [SerializeField] private GameObject _roomListingEntryPrefab;
        [SerializeField] private Transform _roomMenuContent;
    
        private readonly Dictionary<string, RoomListingEntry> _cachedRoomList = new Dictionary<string, RoomListingEntry>();


        private RoomListingEntry CreateRoomListingEntry(RoomInfo roomInfo)
        {
            var roomListing = Instantiate(_roomListingEntryPrefab, _roomMenuContent).GetComponent<RoomListingEntry>();
            roomListing.RoomInfo = roomInfo;
            roomListing.Initialize();
            return roomListing;
        }
        
        public void UpdateRoomListing(List<RoomInfo> roomList)
        {
            for(int i=0; i<roomList.Count; i++)
            {
                RoomInfo info = roomList[i];
                if (info.RemovedFromList)
                {
                    Destroy(_cachedRoomList[info.Name].gameObject);
                    _cachedRoomList.Remove(info.Name);
                }
                else
                {
                    _cachedRoomList[info.Name] = CreateRoomListingEntry(info);
                }
            }
        }

        
    }
}
