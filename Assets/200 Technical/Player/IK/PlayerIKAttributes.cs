// ===== Enhanced Editor - https://github.com/LucasJoestar/LudumDare50 ===== //
//
// ============================================================================ //

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
        public Vector2 BoneLengthMinMax = Vector2.one;
        public AnimationCurve SquishCurve = AnimationCurve.Linear(0, 0, 1, 1);
        public AnimationCurve StretchCurve = AnimationCurve.Linear(0, 0, 1, 1);

        [Section("Jump")]
        public Vector2 HorizontalOffsetMinMax = Vector2.one;
        public AnimationCurve JumpCurve = AnimationCurve.Linear(0,0,1,1);

        [Section("Landing")]
        public AnimationCurve LandingCurve = AnimationCurve.Linear(0,0,1,1);
        public AnimationCurve InstabilityCurve = AnimationCurve.Linear(0,0,1,1);
        public float InstabilityMax = 45.0f;
        
        
        #endregion
    }
}
