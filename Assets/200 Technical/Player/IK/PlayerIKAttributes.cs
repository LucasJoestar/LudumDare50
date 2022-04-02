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
        public float BoneLengthMax = 1.0f;
        public AnimationCurve SquashCurve = AnimationCurve.Linear(0, 0, 1, 1);
        public float SquashMultiplier = .1f;
        public AnimationCurve StretchCurve = AnimationCurve.Linear(0, 0, 1, 1);

        [Section("Jump")]
        public float HorizontalOffset = .1f;
        public AnimationCurve JumpCurve = AnimationCurve.Linear(0,0,1,1);

        [Section("Landing")]
        public AnimationCurve LandingCurve = AnimationCurve.Linear(0,0,1,1);
        public AnimationCurve InstabilityCurve = AnimationCurve.Linear(0,0,1,1);
        public float InstabilityMax = 45.0f;
        
        
        #endregion
    }
}
