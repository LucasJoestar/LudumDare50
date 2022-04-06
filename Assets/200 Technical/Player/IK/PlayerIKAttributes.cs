// ===== Enhanced Editor - https://github.com/LucasJoestar/LudumDare50 ===== //
//
// ============================================================================ //

using DG.Tweening;
using EnhancedEditor;
using UnityEngine;
using Range = EnhancedEditor.RangeAttribute;

namespace LudumDare50
{
    [CreateAssetMenu(fileName = "PlayerIKAttributes", menuName = "LudumDare50/PlayerIKAttributes")]
	public class PlayerIKAttributes : ScriptableObject
    {
        #region Global Members
        [Section("Stretch / Squish ")]
        public AnimationCurve SquashCurve = AnimationCurve.Linear(0, 0, 1, 1);
        public float SquashMultiplier = .1f;
        public AnimationCurve StretchCurve = AnimationCurve.Linear(0, 0, 1, 1);
        public float BoneLengthMax = 1.0f;

        [Section("Jump")]
        public float HorizontalOffset = .1f;
        public AnimationCurve HorizontalJumpCurve = AnimationCurve.Linear(0,0,1,1);

        [Section("Landing")]
        public AnimationCurve HorizontalLandingCurve = AnimationCurve.Linear(0,0,1,1);
        public AnimationCurve VerticalLandingCurve = AnimationCurve.Linear(0,0,1,1);
        // -- // 
        public float InstabilityMax = 45.0f;
        
        [Section("Get Ingredient")]
        public AnimationCurve HorizontalIngredientCurve = AnimationCurve.Linear(0,0,1,1);

        [Section("Splash")]
        public Ease SplashEase = Ease.InOutBack;
        #endregion
    }
}
