// ===== Enhanced Editor - https://github.com/LucasJoestar/LudumDare50 ===== //
//
// ============================================================================ //

using DG.Tweening;
using EnhancedEditor;
using UnityEngine;

using Range = EnhancedEditor.RangeAttribute;

namespace LudumDare50 {
    [CreateAssetMenu(fileName = "ScoreUIFeedbackAttributes", menuName = "LudumDare50/ScoreUIFeedbackAttributes")]
	public class ScoreUIFeedbackAttributes : ScriptableObject {
        #region Global Members
        [Section("Score UI Feedback Attributes")]

        [Enhanced, Range(0f, 1f)] public float BaseAlpha = 0f;
        [Enhanced, Range(0f, 10f)] public float BaseOffset = 1f;
        public Vector3 BaseScale = Vector3.one;

        [Space(10f)]

        [Enhanced, Range(0f, 10f)] public float MoveOffset = 1f;
        [Enhanced, Range(0f, 5f)] public float MoveOffsetDelay = .1f;
        [Enhanced, Range(0f, 5f)] public float MoveOffsetDuration = .1f;
        public Ease MoveOffsetEase = Ease.Linear;

        [Space(10f)]

        public Vector3 ResizeScale = Vector3.one;
        [Enhanced, Range(0f, 5f)] public float ResizeScaleDelay = .1f;
        [Enhanced, Range(0f, 5f)] public float ResizeScaleDuration = .1f;
        public AnimationCurve ResizeScaleCurve = AnimationCurve.Linear(0f, 0f, 1f, 0f);

        [Space(10f)]

        [Enhanced, Range(0f, 5f)] public float FadeDelay = .1f;
        [Enhanced, Range(0f, 5f)] public float FadeDuration = .1f;
        public AnimationCurve FadeCurve = AnimationCurve.Linear(0f, 0f, 1f, 0f);

        [Space(10f)]

        public Gradient Gradient = new Gradient();
        [Enhanced, Range(0f, 5f)] public float GradientDelay = .1f;
        [Enhanced, Range(0f, 5f)] public float GradientDuration = .1f;
        public Ease GradientEase = Ease.Linear;
        #endregion
    }
}
