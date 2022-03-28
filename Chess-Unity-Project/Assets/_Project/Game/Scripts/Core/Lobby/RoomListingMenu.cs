using System.Collections.Generic;
using Photon.Realtime;
using UnityEngine;
using Zenject;

namespace SteampunkChess
{
    public class RoomListingMenu : MonoBehaviour
    {
        [SerializeField] private GameObject _roomListingEntryPrefab;
        [SerializeField] private Transform _roomMenuContent;
    
        private readonly Dictionary<string, RoomListingEntry> _cachedRoomListEntries = new Dictionary<string, RoomListingEntry>();
        private RoomListingEntryFactory _roomListingEntryFactory;

        [Inject]
        private void Construct(RoomListingEntryFactory roomListingEntryFactory)
        {
            _roomListingEntryFactory = roomListingEntryFactory;
        }

       
        public void UpdateRoomListing(List<RoomInfo> roomList)
        {
            for(int i=0; i<roomList.Count; i++)
            {
                RoomInfo info = roomList[i];
                if (info.RemovedFromList)
                {
                    Destroy(_cachedRoomListEntries[info.Name].gameObject);
                    _cachedRoomListEntries.Remove(info.Name);
                }
                else
                {
                    _cachedRoomListEntries[info.Name] = _roomListingEntryFactory.CreateRoomListingEntry(info,_roomMenuContent);
                }
            }
        }
    }
}
