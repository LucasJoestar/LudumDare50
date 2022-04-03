// ===== Enhanced Editor - https://github.com/LucasJoestar/LudumDare50 ===== //
//
// ============================================================================ //

using DG.Tweening;
using EnhancedEditor;
using UnityEngine;

using Range = EnhancedEditor.RangeAttribute;

namespace LudumDare50 {
    [CreateAssetMenu(fileName = "UIButtonAttributes", menuName = "LudumDare50/UIButtonAttributes")]
	public class UIButtonAttributes : ScriptableObject {
        [Section("UIButtonAttributes")]

        #region Button
        [BeginFoldout("Button"), Section("Button")]

        public Color ButtonDefaultColor = Color.white;
        [Enhanced, Range(0f, 10f)] public float ButtonDefaultColorDuration = .1f;
        public Ease ButtonDefaultColorEase = Ease.InSine;

        [Space(10f)]

        public Color ButtonSelectedColor = Color.white;
        [Enhanced, Range(0f, 10f)] public float ButtonSelectedColorDuration = .1f;
        public Ease ButtonSelectedColorEase = Ease.OutSine;

        [Space(5f), HorizontalLine(SuperColor.Aquamarine), Space(5f)]

        [Enhanced] public Vector3 ButtonScaleStartIdle = Vector3.one;
        [Enhanced] public Vector3 ButtonScaleEndIdle = Vector3.one;
        [Enhanced, Range(0f, 10f)] public float ButtonScaleIdleDuration = .1f;

        public Ease ButtonScaleIdleEase = Ease.InSine;

        [Space(5f), HorizontalLine(SuperColor.Aquamarine), Space(5f)]

        [Enhanced, Range(0f, 5f)] public float ButtonBlinkDuration = .5f;

        [Enhanced, EndFoldout] public AnimationCurve ButtonBlinkCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 0f);
        #endregion
    }
}
