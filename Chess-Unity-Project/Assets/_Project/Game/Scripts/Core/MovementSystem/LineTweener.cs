using System.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace SteampunkChess
{
    public class LineTweener : IObjectTweener
    {
        private readonly float _duration;

        public LineTweener(float duration)
        {
            _duration = duration;
        }

        public Task MoveTo(Transform transform, Vector3 targetPosition)
        {
            Sequence moveSequence = DOTween.Sequence();
            float distance = Vector3.Distance(targetPosition, transform.position);
            return transform.DOMove(targetPosition, distance / _duration).AsyncWaitForCompletion();
        }
    }
}
