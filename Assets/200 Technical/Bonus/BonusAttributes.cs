// ===== Enhanced Editor - https://github.com/LucasJoestar/LudumDare50 ===== //
//
// ============================================================================ //

using DG.Tweening;
using EnhancedEditor;
using UnityEngine;

using Range = EnhancedEditor.RangeAttribute;

namespace LudumDare50 {
    [CreateAssetMenu(fileName = "BonusAttributes", menuName = "LudumDare50/BonusAttributes")]
	public class BonusAttributes : ScriptableObject {
        [Section("BonusAttributes")]

        #region Spawn
        [BeginFoldout("Spawn"), Section("Spawn")]

        [Enhanced] public Vector3 SpawnScale = Vector3.one;
        [Enhanced, Range(0f, 1f)] public float SpawnScaleDelay = .1f;
        [Enhanced, Range(0f, 5f)] public float SpawnScaleDuration = .1f;
        [Enhanced, EndFoldout] public Ease SpawnScaleEase = Ease.OutBack;
        #endregion

        #region Idle
        [BeginFoldout("Idle"), Section("Idle")]

        [Enhanced] public Vector3 IdleScale = Vector3.one;
        [Enhanced, Range(0f, 5f)] public float IdleScaleDuration = .1f;
        public AnimationCurve IdleScaleCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 0f);

        [Space(10f)]

        [Enhanced, Range(0f, 5f)] public float IdleMovement = .1f;
        [Enhanced, Range(0f, 5f)] public float IdleMovementDuration = .1f;
        public AnimationCurve IdleMovementCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 0f);

        [Space(5f), HorizontalLine(SuperColor.Aquamarine), Space(5f)]

        [Enhanced, Range(-360f, 360f)] public float FeedbackRotation = .1f;
        [Enhanced, Range(0f, 60f)] public float FeedbackRotationDuration = .1f;
        public AnimationCurve FeedbackRotationCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 0f);

        [Space(10f)]

        [Enhanced, Range(0f, 5f)] public float FeedbackMovement = .1f;
        [Enhanced, Range(0f, 5f)] public float FeedbackMovementDuration = .1f;
        public AnimationCurve FeedbackMovementCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 0f);

        [Space(5f), HorizontalLine(SuperColor.Aquamarine), Space(5f)]

        public Gradient FeedbackGradient = new Gradient();
        [Enhanced, Range(0f, 5f)] public float FeedbackGradientDuration = .1f;
        public Ease FeedbackGradientEase = Ease.InSine;

        [Space(10f)]

        public Vector3 FeedbackScale = Vector3.one;
        [Enhanced, Range(0f, 5f)] public float FeedbackScaleDuration = .1f;
        public AnimationCurve FeedbackScaleCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 0f);

        [Space(5f), HorizontalLine(SuperColor.Aquamarine), Space(5f)]

        public Gradient FeedbackOutlineGradient = new Gradient();
        [Enhanced, Range(0f, 5f)] public float FeedbackOutlineGradientDuration = .1f;
        public Ease FeedbackOutlineGradientEase = Ease.InSine;

        [Space(10f)]

        public Vector3 FeedbackOutlineScale = Vector3.one;
        [Enhanced, Range(0f, 5f)] public float FeedbackOutlineScaleDuration = .1f;
        [Enhanced, EndFoldout] public AnimationCurve FeedbackOutlineScaleCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 0f);
        #endregion

        #region Collect
        [BeginFoldout("Collect"), Section("Collect")]

        [Enhanced] public Vector3 CollectScale = Vector3.one;
        [Enhanced, Range(0f, 1f)] public float CollectScaleDelay = .1f;
        [Enhanced, Range(0f, 5f)] public float CollectScaleDuration = .1f;
        public AnimationCurve CollectScaleCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 0f);

        [Space(10f)]

        [Enhanced, Range(0f, 5f)] public float CollectMovement = 0f;
        [Enhanced, Range(0f, 1f)] public float CollectMovementDelay = .1f;
        [Enhanced, Range(0f, 5f)] public float CollectMovementDuration = .1f;
        public AnimationCurve CollectMovementCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 0f);

        [Space(10f)]

        [Enhanced, Range(0f, 1f)] public float CollectFade = 1f;
        [Enhanced, Range(0f, 1f)] public float CollectFadeDelay = .1f;
        [Enhanced, Range(0f, 5f)] public float CollectFadeDuration = .1f;
        public Ease CollectFadeEase = Ease.OutSine;

        [Space(5f), HorizontalLine(SuperColor.Aquamarine), Space(5f)]

        public Vector3 CollectFeedbackScale = Vector3.one;
        [Enhanced, Range(0f, 1f)] public float CollectFeedbackScaleDelay = .1f;
        [Enhanced, Range(0f, 5f)] public float CollectFeedbackScaleDuration = .1f;
        public Ease CollectFeedbackScaleEase = Ease.Linear;

        [Space(10f)]

        [Enhanced, Range(0f, 2f)] public float CollectFeedbackFadeScaleDelay = .1f;
        [Enhanced, Range(0f, 5f)] public float CollectFeedbackFadeDelay = .1f;
        [Enhanced, Range(0f, 5f)] public float CollectFeedbackFadeDuration = .1f;
        [Enhanced, EndFoldout] public Ease CollectFeedbackFadeEase = Ease.OutSine;
        #endregion
    }
}
