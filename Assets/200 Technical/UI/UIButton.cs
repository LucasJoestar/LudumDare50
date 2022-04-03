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
	public class UIButton : Selectable, ISubmitHandler {
        #region Global Members
        [Section("UI Button")]

        [SerializeField, Enhanced, Required] private UIButtonAttributes attributes = null;
        [SerializeField, Enhanced, Required] private RectTransform rectTransform = null;
        [SerializeField, Enhanced, Required] private Image buttonImage = null;
        [SerializeField, Enhanced, Required] private CanvasGroup flash = null;

        [Space(10f)]

        [SerializeField] private UnityEvent onClick = null;
        #endregion

        #region Behaviour
        private Sequence sequence = null;
        private Sequence colorSequence = null;

        private static bool isSubmitting = false;

        // ---------------

        public override void OnSelect(BaseEventData eventData) {
            if (isSubmitting)
                return;

            base.OnSelect(eventData);

            // Animation.
            if (sequence.IsActive()) {
                sequence.Kill(false);
            }

            sequence = DOTween.Sequence(); {
                sequence.Join(rectTransform.DOScale(attributes.ButtonScaleStartIdle, .1f));
                sequence.OnComplete(OnComplete);
            }

            // Color.
            if (colorSequence.IsActive()) {
                colorSequence.Kill(false);
            }

            colorSequence = DOTween.Sequence(); {
                colorSequence.Join(image.DOColor(attributes.ButtonSelectedColor, attributes.ButtonSelectedColorDuration).SetEase(attributes.ButtonSelectedColorEase));
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
                Select();
            }
        }

        public override void OnDeselect(BaseEventData eventData) {
            if (isSubmitting)
                return;

            base.OnDeselect(eventData);

            // Animation.
            if (sequence.IsActive()) {
                sequence.Kill(false);
            }

            sequence = DOTween.Sequence(); {
                sequence.Join(rectTransform.DOScale(1f, attributes.ButtonDefaultColorDuration).SetEase(attributes.ButtonDefaultColorEase));
            }

            // Color.
            if (colorSequence.IsActive()) {
                colorSequence.Kill(false);
            }

            colorSequence = DOTween.Sequence(); {
                colorSequence.Join(image.DOColor(attributes.ButtonDefaultColor, attributes.ButtonDefaultColorDuration).SetEase(attributes.ButtonDefaultColorEase));
            }
        }
        #endregion
    }
}
