using System.Threading.Tasks;
using UnityEngine;

namespace SteampunkChess
{
    public interface IObjectTweener
    {
        Task MoveTo(Transform transform, Vector3 targetPosition);
    }
}

