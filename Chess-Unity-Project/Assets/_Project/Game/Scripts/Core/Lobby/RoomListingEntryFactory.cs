using Photon.Realtime;
using UnityEngine;
using Zenject;

namespace SteampunkChess
{
    public class RoomListingEntryFactory
    {
        private readonly IInstantiator _instantiator;
        private readonly RoomListingEntry _roomListingEntryPrefab;


        public RoomListingEntryFactory(IInstantiator instantiator, RoomListingEntry roomListingEntryPrefab)
        {
            _instantiator = instantiator;
            _roomListingEntryPrefab = roomListingEntryPrefab;
        }

        public RoomListingEntry CreateRoomListingEntry(RoomInfo roomInfo, Transform roomMenuContent)
        {
            var roomListing = _instantiator.InstantiatePrefab(_roomListingEntryPrefab.gameObject, roomMenuContent).GetComponent<RoomListingEntry>();
            roomListing.RoomInfo = roomInfo;
            roomListing.Initialize();
            return roomListing;
        }
    }
}