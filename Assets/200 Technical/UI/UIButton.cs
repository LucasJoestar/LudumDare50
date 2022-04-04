// ===== Enhanced Editor - https://github.com/LucasJoestar/LudumDare50 ===== //
//
// ============================================================================ //

using DG.Tweening;
using EnhancedEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace LudumDare50 {
	public class UIButton : MonoBehaviour, ISubmitHandler, ISelectHandler, IDeselectHandler {
        #region Global Members
        [Section("UI Button")]

        [SerializeField, Enhanced, Required] public Selectable Selectable = null;
        [SerializeField, Enhanced, Required] private UIButtonAttributes attributes = null;
        [SerializeField, Enhanced, Required] private RectTransform rectTransform = null;
        [SerializeField, Enhanced, Required] private Image buttonImage = null;
        [SerializeField, Enhanced, Required] private CanvasGroup flash = null;
        [SerializeField, Enhanced, Required] private AudioClip selectClip = null;
        [SerializeField, Enhanced, Required] private AudioClip clickClip = null;
        [Space(10f)]

        [SerializeField] private UnityEvent onClick = null;
        #endregion

        #region Behaviour
        private static bool isSubmitting = false;

        private Sequence sequence = null;
        private Sequence colorSequence = null;

        private Vector3 baseScale = Vector3.one;

        // ---------------

        protected void OnEnable() {
            baseScale = rectTransform.localScale;
        }

        public void OnSelect(BaseEventData eventData) {
            if (isSubmitting)
                return;

            SoundManager.Instance.PlayClip(selectClip);

            // Animation.
            if (sequence.IsActive()) {
                sequence.Kill(false);
            }

            sequence = DOTween.Sequence(); {
                sequence.Join(rectTransform.DOScale(attributes.ButtonScaleStartIdle, attributes.ButtonScaleStartIdleDuration).SetEase(attributes.ButtonScaleStartIdleEase));
                sequence.OnComplete(OnComplete);
            }

            // Color.
            if (colorSequence.IsActive()) {
                colorSequence.Kill(false);
            }

            colorSequence = DOTween.Sequence(); {
                colorSequence.Join(buttonImage.DOColor(attributes.ButtonSelectedColor, attributes.ButtonSelectedColorDuration).SetEase(attributes.ButtonSelectedColorEase));
            }

            // ----- Local Method ----- \\

            void OnComplete() {
                sequence = DOTween.Sequence(); {
                    sequence.Join(rectTransform.DOScale(attributes.ButtonScaleEndIdle, attributes.ButtonScaleIdleDuration).SetEase(attributes.ButtonScaleIdleEase));
                    sequence.SetLoops(-1, LoopType.Yoyo);
                }
            }
        }

        public void OnSubmit(BaseEventData eventData) {
            if (isSubmitting)
                return;

            isSubmitting = true;
            UIManager.Instance.EnableButtons(false);

            SoundManager.Instance.PlayClip(clickClip);

            // Animation.
            if (sequence.IsActive()) {
                sequence.Kill(true);
            }

            sequence = DOTween.Sequence(); {
                sequence.Join(flash.DOFade(1f, attributes.ButtonBlinkDuration).SetEase(attributes.ButtonBlinkCurve));
                sequence.OnComplete(OnComplete);
            }

            // ----- Local Method ----- \\

            void OnComplete() {
                onClick.Invoke();

                isSubmitting = false;
                UIManager.Instance.EnableButtons(true);
                Selectable.Select();
            }
        }

        public void OnDeselect(BaseEventData eventData) {
            if (isSubmitting)
                return;

            // Animation.
            if (sequence.IsActive()) {
                sequence.Kill(false);
            }

            sequence = DOTween.Sequence(); {
                sequence.Join(rectTransform.DOScale(baseScale, attributes.ButtonDefaultScaleDuration).SetEase(attributes.ButtonDefaultScaleEase));
            }

            // Color.
            if (colorSequence.IsActive()) {
                colorSequence.Kill(false);
            }

            colorSequence = DOTween.Sequence(); {
                colorSequence.Join(buttonImage.DOColor(attributes.ButtonDefaultColor, attributes.ButtonDefaultColorDuration).SetEase(attributes.ButtonDefaultColorEase));
            }
        }
        #endregion
    }
}
