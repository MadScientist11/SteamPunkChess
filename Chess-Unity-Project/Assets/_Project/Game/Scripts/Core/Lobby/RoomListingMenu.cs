using System.Collections.Generic;
using Photon.Realtime;
using UnityEngine;
using Zenject;

namespace SteampunkChess
{
    public class RoomListingMenu : MonoBehaviour
    {
        [SerializeField] private Transform _roomMenuContent;
    
        private readonly Dictionary<string, RoomListingEntry> _cachedRoomListEntries = new Dictionary<string, RoomListingEntry>();
        private RoomListingEntryFactory _roomListingEntryFactory;
        private readonly List<string> toBeRemoved = new List<string>();

        [Inject]
        private void Construct(RoomListingEntryFactory roomListingEntryFactory)
        {
            _roomListingEntryFactory = roomListingEntryFactory;
        }
        
        public void UpdateRoomListing(List<RoomInfo> roomList)
        {
            RemoveNullElements();
            for(int i=0; i<roomList.Count; i++)
            {
                RoomInfo info = roomList[i];
                if (info.RemovedFromList)
                {
                    if (_cachedRoomListEntries.TryGetValue(info.Name, out var entry))
                    {
                        Destroy(entry.gameObject);
                        _cachedRoomListEntries.Remove(info.Name);
                    }
                   
                }
                else
                {
                    _cachedRoomListEntries[info.Name] = _roomListingEntryFactory.CreateRoomListingEntry(info,_roomMenuContent);
                }
            }
        }
        
        private void RemoveNullElements()
        {
            toBeRemoved.Clear();
            foreach(var kv in _cachedRoomListEntries)
            {
                if (kv.Value == null)
                {
                    toBeRemoved.Add(kv.Key);
                }
            }
            for(int i = 0; i < toBeRemoved.Count; i++)
            {
                _cachedRoomListEntries.Remove(toBeRemoved[i]);
            }
            toBeRemoved.Clear();
        }
    }
}
