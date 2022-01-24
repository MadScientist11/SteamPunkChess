using DG.Tweening;
using Sirenix.OdinInspector;
using System;
using UnityEngine;
namespace SteampunkChess.PopUps
{
    public class PopUp : MonoBehaviour, IPopUp
    {
        [SerializeField] private Vector2 _startLocation;
        [SerializeField] private Vector2 _locationTo;

        [SerializeField] private RectTransform _tweenTransform;

        [Header("Animation Tweaks")]
        [SerializeField] private Ease _easeType;
        [SerializeField] private float _showDuration;
        [SerializeField] private float _hideDuration;

        public event Action OnDestroyed;


        private void Start()
        {
            ResetLocationAndScale();
        }

        private void ResetLocationAndScale()
        {
            _tweenTransform.localScale = new Vector3(1, 1, 1);
            _tweenTransform.anchoredPosition = _startLocation;
        }

        [Button, DisableInEditorMode]
        public void Show()
        {
            ResetLocationAndScale();
            _tweenTransform.DOAnchorPos(_locationTo, _showDuration).SetEase(_easeType);
        }

        [Button, DisableInEditorMode]
        public void Hide(bool destroyAfterHide = false)
        {
            _tweenTransform.DOScale(new Vector3(0, 0, 0), _hideDuration).OnComplete(() =>
            {
                if (destroyAfterHide)
                {
                    Destroy(gameObject);
                    OnDestroyed?.Invoke();
                }
            });
        }


    }
}
