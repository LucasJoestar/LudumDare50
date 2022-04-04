// ===== Enhanced Editor - https://github.com/LucasJoestar/LudumDare50 ===== //
//
// ============================================================================ //

using DG.Tweening;
using EnhancedEditor;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace LudumDare50 {
	public class ScoreUIFeedback : MonoBehaviour {
        #region Global Members
        [Section("ScoreUIFeedback")]

        [SerializeField, Enhanced, Required] private CanvasGroup group = null;
        [SerializeField, Enhanced, Required] private TextMeshProUGUI text = null;
        #endregion

        #region Behaviour
        private Sequence sequence = null;
        private float gradientVar = 0f;

        // ---------------

        public void Initialize(ScoreUIFeedbackAttributes attributes, float score) {
            transform.position = PlayerController.Instance.transform.position + (Vector3.up * attributes.BaseOffset);
            transform.rotation = Quaternion.identity;
            transform.localScale = attributes.BaseScale;

            text.text = score.ToString("### ### ###").Trim();
            group.alpha = attributes.BaseAlpha;

            sequence = DOTween.Sequence(); {
                sequence.Join(transform.DOMoveY(transform.position.y + attributes.MoveOffset, attributes.MoveOffsetDuration).SetEase(attributes.MoveOffsetEase)
                        .SetDelay(attributes.MoveOffsetDelay));

                sequence.Join(transform.DOScale(attributes.ResizeScale, attributes.ResizeScaleDuration).SetEase(attributes.ResizeScaleCurve).SetDelay(attributes.ResizeScaleDelay));
                sequence.Join(group.DOFade(1f, attributes.FadeDuration).SetEase(attributes.FadeCurve).SetDelay(attributes.FadeDelay));

                // Gradient.
                Sequence gradient = DOTween.Sequence(); {
                    SetGradient(0f);

                    gradient.Join(DOTween.To(GetGradient, SetGradient, 1f, attributes.GradientDuration).SetEase(attributes.GradientEase).SetDelay(attributes.GradientDelay));
                    sequence.Join(gradient);

                    // ----- Local Methods ----- \\

                    float GetGradient() {
                        return gradientVar;
                    }

                    void SetGradient(float value) {
                        text.color = attributes.Gradient.Evaluate(value);
                        gradientVar = value;
                    }
                };

                sequence.OnComplete(OnComplete);
            }

            // ----- Local Methods ----- \\

            void OnComplete() {
                UIManager.Instance.OnFeedbackComplete(this);
                Complete();
            }
        }

        public void Complete() {
            sequence.Kill(false);
            Destroy(gameObject);
        }
        #endregion
    }
}
