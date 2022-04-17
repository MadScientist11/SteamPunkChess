using DG.Tweening;
using Sirenix.OdinInspector;
using System;
using SteampunkChess.PopUpService;
using UnityEngine;
using Zenject;

namespace SteampunkChess.PopUps
{
    public abstract class PopUp : MonoBehaviour, IPopUp
    {
        [SerializeField] private Vector2 _startLocation;
        [SerializeField] private Vector2 _locationTo;

        [SerializeField] private RectTransform _tweenTransform;

        [Header("Animation Tweaks")] 
        [SerializeField] private Ease _easeType;

        [SerializeField] private float _showDuration;
        [SerializeField] private float _hideDuration;
        private IPopUpService _popUpService;

        public bool IsVisible { get; private set; }

        public event Action OnDestroyed;


        public abstract string PopUpKey { get; set; }

        [Inject]
        private void Construct(IPopUpService popUpService)
        {
            _popUpService = popUpService;
        }


        public virtual void Start()
        {
            ResetLocationAndScale();
        }

        private void ResetLocationAndScale()
        {
            _tweenTransform.localScale = new Vector3(1, 1, 1);
            _tweenTransform.anchoredPosition = _startLocation;
        }

        [Button, DisableInEditorMode]
        public virtual void Show(params object[] data)
        {
            IsVisible = true;
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
                }
            });
        }

        private void OnDestroy()
        {
            _popUpService.RemoveInstanceFromInternalPool(PopUpKey);
            OnDestroyed?.Invoke();
        }
    }
}