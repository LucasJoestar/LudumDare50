// ===== Enhanced Editor - https://github.com/LucasJoestar/LudumDare50 ===== //
//
// ============================================================================ //

using DG.Tweening;
using EnhancedEditor;
using UnityEngine;

using Range = EnhancedEditor.RangeAttribute;

namespace LudumDare50 {
    [CreateAssetMenu(fileName = "UIManagerAttributes", menuName = "LudumDare50/UIManagerAttributes")]
	public class UIManagerAttributes : ScriptableObject {
        [Section("UI Manager Attributes")]

        #region Menu Transitions
        [BeginFoldout("Menu Transitions"), Section("Menu Transitions")]

        [Enhanced, Range(0f, 1f)] public float MenuFadeInBlackDelay = 0f;
        [Enhanced, Range(0f, 5f)] public float MenuFadeInBlackDuration = .1f;
        public Ease MenuFadeInBlackEase = Ease.OutSine;

        [Space(10f)]

        [Enhanced, Range(0f, 5f)] public float MenuFadeOutBlackDelay = 0f;
        [Enhanced, Range(0f, 5f)] public float MenuFadeOutBlackDuration = .1f;
        public Ease MenuFadeOutBlackEase = Ease.InSine;

        [Space(5f), HorizontalLine(SuperColor.Crimson), Space(5f)]

        [Enhanced, Range(0f, 5f)] public float MenuFadeInDelay = 0f;
        [Enhanced, Range(0f, 5f)] public float MenuFadeInDuration = .1f;
        public Ease MenuFadeInEase = Ease.OutSine;

        [Space(10f)]

        [Enhanced, Range(0f, 1f)] public float MenuFadeOutDelay = 0f;
        [Enhanced, Range(0f, 5f)] public float MenuFadeOutDuration = .1f;

        [Enhanced, EndFoldout] public Ease MenuFadeOutEase = Ease.InSine;
        #endregion

        #region Title
        [BeginFoldout("Title"), Section("Title")]

        [Enhanced, Range(0f, 5f)] public float TitleInDelay = 0f;
        [Enhanced, Range(0f, 5f)] public float TitleFadeInDuration = .1f;
        public Ease TitleFadeInEase = Ease.OutSine;

        [Space(10f)]

        [Enhanced] public Vector3 TitleScaleInSize = Vector3.one;
        [Enhanced, Range(0f, 5f)] public float TitleScaleInDuration = .1f;
        public Ease TitleScaleInEase = Ease.OutSine;

        [Space(10f)]

        [Enhanced, Range(0f, 10f)] public float TitleRotateInLoop = 1f;
        [Enhanced, Range(0f, 5f)] public float TitleRotateInDuration = .1f;
        public Ease TitleRotateInEase = Ease.OutSine;

        [Space(10f), HorizontalLine(SuperColor.Sapphire), Space(5f)]

        [Enhanced] public Vector3 TitleScaleIdle = Vector3.one;
        [Enhanced, Range(0f, 10f)] public float TitleScaleIdleDuration = .1f;
        public AnimationCurve TitleScaleIdleCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 0f);

        [Space(10f)]

        [Enhanced, Range(-100f, 100f)] public float TitleMoveIdleFrom = 1f;
        [Enhanced, Range(-100f, 100f)] public float TitleMoveIdle = 1f;
        [Enhanced, Range(0f, 10f)] public float TitleMoveIdleDuration = .1f;
        public AnimationCurve TitleMoveIdleCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 0f);

        [Space(5f), HorizontalLine(SuperColor.Sapphire), Space(5f)]

        [Enhanced, Range(0f, 1f)] public float TitleOutDelay = 0f;
        [Enhanced, Range(0f, 5f)] public float TitleFadeOutDuration = .1f;
        public Ease TitleFadeOutEase = Ease.OutSine;

        [Space(10f)]

        [Enhanced] public Vector3 TitleScaleOut = Vector3.one;
        [Enhanced, Range(0f, 5f)] public float TitleScaleOutDuration = .1f;
        public Ease TitleScaleOutEase = Ease.OutSine;

        [Space(10f)]

        [Enhanced, Range(0f, 10f)] public float TitleRotateOutLoop = 1f;
        [Enhanced, Range(0f, 5f)] public float TitleRotateOutDuration = .1f;
        [Enhanced, EndFoldout] public Ease TitleRotateOutEase = Ease.InSine;
        #endregion

        #region Score

        #endregion
    }
}
