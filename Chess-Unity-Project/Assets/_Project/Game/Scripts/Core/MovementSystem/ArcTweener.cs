using System.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace SteampunkChess
{
    public class ArcTweener : IObjectTweener
    {
        private readonly float _movementSpeed;
        private readonly float _jumpHeight;

        public ArcTweener(float movementSpeed, float jumpHeight)
        {
            _movementSpeed = movementSpeed;
            _jumpHeight = jumpHeight;
        }
        
        public Task MoveTo(Transform transform, Vector3 targetPosition)
        {
            Sequence moveSequence = DOTween.Sequence();
            float distance = Vector3.Distance(targetPosition, transform.position);

            return transform.DOJump(targetPosition, _jumpHeight, 1, distance / _movementSpeed).AsyncWaitForCompletion();
        }
    }
}