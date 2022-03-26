using Sirenix.OdinInspector;
using UnityEngine;

namespace SteampunkChess
{
    public class RoomListingEntryMediator : MonoBehaviour
    {
        [SerializeField] private RoomListingEntry _roomListingEntry;

        [Button, DisableInEditorMode] public void OnClick_Join() => _roomListingEntry.JoinRoom();
    }
}